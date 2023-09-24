using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayPlanner_ConsoleApp
{
    class Repository
    {
        /// <summary>
        /// основной иассив записей о мероприятиях
        /// </summary>
        private MyEntry[] arrEntries; // Основной массив для хранения данных

        /// <summary>
        /// путь к файлу с данными
        /// </summary>
        private string path; // путь к файлу с данными

        /// <summary>
        /// номер следующего элемента для добавления в массив (число записей в массиве)
        /// </summary>
        private int index; // текущий элемент для добавления в workers

        /// <summary>
        /// разделитель данных в строке файла в CSV формате
        /// </summary>
        private char sep;   // разделитель данных в строке

        /// <summary>
        /// массив, хранящий заголовки полей
        /// </summary>
        private string[] titles; // массив, хранящий заголовки полей. используется в PrintDbToConsole

        /// <summary>
        /// формат вывода записей
        /// </summary>
        private string entryPatern = "| {0, -2} | {1, -10} | {2, -10} | {3, -16} | {4, -20} | {5, 12} | {6, -4} | {7, -14} |";

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Path">путь и имя файла</param>
        /// <param name="sep">разделитель в CSV файле</param>
        public Repository(string Path, char sep = ',')
        {
            this.path = Path; // Сохранение пути к файлу с данными
            this.index = 0; // текущая позиция для добавления сотрудника в workers
            this.sep = sep;
            this.titles = new string[0]; // инициализаия массива заголовков   
            this.arrEntries = new MyEntry[0]; // инициализаия массива сотрудников.    | изначально предпологаем, что данных нет

            this.Load(); // Загрузка данных
        }

        /// <summary>
        /// Установить формат вывода записей о мероприятиях
        /// </summary>
        /// <param name="patern">формат вывода записей</param>
        public void SetPatern(string patern)
        {
            entryPatern = patern;
        }

        /// <summary>
        /// Добавление новой заполненной записи о мероприятии
        /// </summary>
        /// <param name="newEntry">запись о мероприятии</param>
        public void Add(MyEntry newEntry)
        {
            if (index >= this.arrEntries.Length) Array.Resize(ref this.arrEntries, this.arrEntries.Length + 1);
            this.arrEntries[index] = newEntry;
            this.index++;
        }

        /// <summary>
        /// Редактирование информации о мероприятии
        /// </summary>
        /// <param name="id">ID в списке</param>
        /// <param name="mode">какое поле редактируем</param>
        /// <returns>информация об ошибке при наличии</returns>
        public string Edit(int id, string mode)
        {
            MyEntry me = null;
            for (int i = 0; i < index; i++)
            {
                if (arrEntries[i].ID == id)
                {
                    me = arrEntries[i];
                    break;
                }
            }
            if (me == null) 
            {
                return "Мероприятия с идентификатором " + id.ToString() + " нет в базе"; 
            }
            else
            {
                switch(mode)
                {
                    case "2":
                        Console.WriteLine(me.Date + ". Введите новую дату мероприятия (пример : 01.01.1970 )");
                        me.Date = Console.ReadLine();
                        break;
                    case "3":
                        Console.WriteLine(me.Time + ". Введите новое время начала мероприятия (пример : 14:30 )");
                        me.Time = Console.ReadLine();
                        break;
                    case "4":
                        Console.WriteLine(me.Place + ". Введите другое место проведения мероприятия (пример : кабинет )");
                        me.Place = Console.ReadLine();
                        break;
                    case "5":
                        Console.WriteLine(me.EventName + ". Введите новое наименование мероприятия (пример : совещание )");
                        me.EventName = Console.ReadLine();
                        break;
                    case "6":
                        Console.WriteLine(me.Duration + ". Введите новую продолжительность мероприятия в минутах (пример : 30 )");
                        me.Duration = Console.ReadLine();
                        break;
                    case "7":
                        Console.WriteLine(me.Completion + ". Введите отметку о выполнении мероприятия (пример : да или нет )");
                        me.Completion = Console.ReadLine();
                        break;
                    case "8":
                        Console.WriteLine(me.Remark + "Введите новое примечание (пример : - )");
                        me.Remark = Console.ReadLine();
                        break;
                    default:
                        return "Ошибка ввода номкра команды : " + mode;
                }
            }
            return "";
        }

        /// <summary>
        /// Переназначение идентификаторов в порядке нахождения записей в массиве
        /// </summary>
        private void ReCalcID()
        {
            for (int i = 0; i < index; i++) arrEntries[i].ID = i + 1;
        }

        /// <summary>
        /// Метод получения мероприятия по запрашиваемому ID
        /// </summary>
        /// <param name="id">идентификатор мероприятия</param>
        /// <returns>экземпляр класса MyEntry или null</returns>
        public MyEntry GetEntryById(int id)
        {
            // происходит чтение из файла, возвращается Worker
            // с запрашиваемым ID
            for (int i = 0; i < index; i++)
            {
                if (arrEntries[i].ID == id) return arrEntries[i];
            }
            return null;
        }

        /// <summary>
        /// Удаление мероприятия по его ID в списке
        /// </summary>
        /// <param name="id">ID в списке</param>
        /// <returns>true - найден и удалён, false - ID не найден</returns>
        public bool DeleteEntry(int id)
        {
            bool res = false;
            MyEntry me = GetEntryById(id);
            if (me != null)
            {
                if (index > 1)
                {
                    MyEntry[] tmpArr = new MyEntry[index - 1];
                    for (int i = 0, j = 0; i < index; i++)
                    {
                        if (arrEntries[i].ID == me.ID)
                        {
                            continue;
                        }
                        else
                        {
                            tmpArr[j++] = arrEntries[i];
                        }
                    }
                    index--;
                    arrEntries = tmpArr;
                    ReCalcID();
                }
                else
                {
                    index = 0;
                    arrEntries = new MyEntry[0];
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Добавление к общему списку мероприятий из файла в CSV формате
        /// </summary>
        /// <param name="fileName">путь и имя файла</param>
        public void AddListFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    sr.ReadLine();
                    int n = 0;
                    while (!sr.EndOfStream)
                    {
                        Add(new MyEntry(sr.ReadLine(), sep));
                        n++;
                    }
                    Console.WriteLine("Добавлено записей : " + n.ToString());
                    Ordering();
                    ReCalcID();
                }
            }
        }

        /// <summary>
        /// Упорядочивание мероприятий в массиве по дате и времени
        /// </summary>
        private void Ordering()
        {
            var tmpArr = arrEntries.OrderBy(me => me, new CustomMyEntryComparer());            
            arrEntries = tmpArr.ToArray<MyEntry>();
        }

        /// <summary>
        /// Загрузка массива мероприятий из заданного в конструкторе файла 
        /// </summary>
        private void Load()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(this.path))
                {
                    titles = sr.ReadLine().Split(sep);

                    while (!sr.EndOfStream)
                    {
                        Add(new MyEntry(sr.ReadLine(), sep));
                    }
                }
            }
            else
            {
                titles = new string[8];
                titles[0] = "ID";
                titles[1] = "Дата";
                titles[2] = "Время";
                titles[3] = "Место";
                titles[4] = "Мероприятие";
                titles[5] = "Длительность";
                titles[6] = "Вып.";
                titles[7] = "Примечание";
            }
        }

        /// <summary>
        /// Сохранение переданного массива мероприятий в указанный файл в формате CSV с заданным разделителем
        /// </summary>
        /// <param name="fileName">путь и имя файла</param>
        public void SaveSelect(string fileName, MyEntry[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                int i;

                StringBuilder sb = new StringBuilder(titles[0]);
                for (i = 1; i < titles.Length; i++)
                {
                    sb.Append(sep + titles[i]);
                }
                string[] records = new string[1 + arr.Length];
                records[0] = sb.ToString();
                for (i = 0; i < arr.Length; i++)
                {
                    records[i + 1] = arr[i].CsvRecord(sep);
                }
                File.WriteAllLines(fileName, records);
            }
        }

        /// <summary>
        /// Сохранение массива мероприятий в указанный файл в формате CSV с заданным разделителем
        /// </summary>
        /// <param name="Path">путь и имя файла</param>
        public void Save(string Path)
        {
            if (index > 0)
            {
                int i;

                StringBuilder sb = new StringBuilder(titles[0]);
                for (i = 1; i < titles.Length; i++)
                {
                    sb.Append(sep + titles[i]);
                }
                string[] records = new string[1 + index];
                records[0] = sb.ToString();
                for (i = 0; i < index; i++)
                {
                    records[i + 1] = arrEntries[i].CsvRecord(sep);
                }
                File.WriteAllLines(Path, records);
            }
        }

        /// <summary>
        /// Получение объекта DateTime из строки вида "25.09.2023 10:00"
        /// </summary>
        /// <param name="s">строка с датой и временем или только с датой</param>
        /// <returns>объект DateTime</returns>
        private DateTime StringToDateTime(string s)
        {
            string[] tail = s.Split(' ');
            string[] param = tail[0].Split('.');
            if (param.Length == 3)
            {
                if (int.TryParse(param[0], out int day) && int.TryParse(param[1], out int month) && int.TryParse(param[2], out int year))
                {
                    DateTime dt = new DateTime(year, month, day);
                    return dt;
                }
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Получение выборочного списка мероприятий по одному из критериев
        /// </summary>
        /// <param name="mode">тип критерия</param>
        public void GetCustomInfo(string mode)
        {
            MyEntry[] arr = new MyEntry[1];
            int currentIndex = 0, i;
            string strInp1, strInp2, strTitle;
            DateTime dt1, dt2, dt3;

            switch (mode)
            {
                case "1":   //  записи в интервале дат заполнения
                    Console.WriteLine("Введите дату начала интервала (пример : 01.01.1970)");
                    strInp1 = Console.ReadLine();
                    Console.WriteLine("Введите дату завершения интервала (пример : 31.12.2023)");
                    strInp2 = Console.ReadLine();
                    dt1 = StringToDateTime(strInp1);
                    dt2 = StringToDateTime(strInp2);
                    for (i = 0; i < index; i++)
                    {
                        dt3 = arrEntries[i].GetDateTime();
                        if (dt3 > dt1 && dt3 < dt2)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = arrEntries[i];
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список мероприятий с " + strInp1 + " по " + strInp2 + " :";
                        PrintArrToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nМероприятий с " + strInp1 + " по " + strInp2 + "найдено не было\n");
                    }
                    break;
                case "2":   //  записи по месту проведения
                    Console.WriteLine("Введите место проведения мероприятия (пример : кабинет или офис)");
                    strInp1 = Console.ReadLine();
                    for (i = 0; i < index; i++)
                    {
                        if (arrEntries[i].Place == strInp1)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = arrEntries[i];
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список мероприятий по месту \"" + strInp1 + "\" :";
                        PrintArrToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nМероприятий по месту \"" + strInp1 + "\" нет\n");
                    }
                    break;
                case "3":   //  записи по наименованию
                    Console.WriteLine("Введите наименование мероприятия (пример : совещание)");
                    strInp1 = Console.ReadLine();
                    for (i = 0; i < index; i++)
                    {
                        if (arrEntries[i].EventName == strInp1)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = arrEntries[i];
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список мероприятий по наименованию \" " + strInp1 + " \" :";
                        PrintArrToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nМероприятий с наименованием \"" + strInp1 + "\" нет\n");
                    }
                    break;
                case "4":   //  не выполненные мероприятия
                    strInp1 = "нет";
                    for (i = 0; i < index; i++)
                    {
                        if (arrEntries[i].Completion == strInp1)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = arrEntries[i];
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список не выполненных мероприятий :";
                        PrintArrToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nНе выполненных мероприятий нет !!!\n");
                    }
                    break;
            }
            if (currentIndex > 0)
            {
                Console.WriteLine("Если хотите сохранить выбранное в отдельный файл,\nто введите его имя (пример: select.csv ), если нет, то нажмите < Enter >");
                strInp1 = Console.ReadLine();
            }
        }

        /// <summary>
        /// Вывод массива всех мероприятий на консоль
        /// </summary>
        public void PrintDbToConsole()
        {
            PrintArrToConsole(arrEntries);
        }

        /// <summary>
        /// Вывод массива мероприятий на консоль 
        /// </summary>
        /// <param name="arr">массив мероприятий</param>
        /// <param name="title">заголовок</param>
        private void PrintArrToConsole(MyEntry[] arr, string title = "Список мероприятий")
        {
            if (arr != null && arr.Length > 0)
            {
                Console.WriteLine("\n" + title + "\n");
                Console.WriteLine(entryPatern, titles[0], titles[1], titles[2], titles[3], titles[4], titles[5], titles[6], titles[7]);

                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine(arr[i].OutFormatString(entryPatern));
                }
            }
            else
            {
                Console.WriteLine("\nСписок мероприятий пуст\n");
            }
        }

        /// <summary>
        /// Количество записей о мероприятиях в хранилище
        /// </summary>
        public int Count { get { return this.index; } }
    }
}
