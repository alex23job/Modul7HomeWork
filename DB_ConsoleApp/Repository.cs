using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_ConsoleApp
{
    /// <summary>
    /// Структура по работе с данными
    /// </summary>
    class Repository
    {
        /// <summary>
        /// основной иассив записей о сотрудниках
        /// </summary>
        private Worker[] workers; // Основной массив для хранения данных

        /// <summary>
        /// путь к файлу с данными
        /// </summary>
        private string path; // путь к файлу с данными

        /// <summary>
        /// номер следующего элемента для добавления в массив (число записей в массиве)
        /// </summary>
        int index; // текущий элемент для добавления в workers

        /// <summary>
        /// разделитель данных в строке файла в CSV формате
        /// </summary>
        char sep;   // разделитель данных в строке

        /// <summary>
        /// массив, хранящий заголовки полей
        /// </summary>
        string[] titles; // массив, храняий заголовки полей. используется в PrintDbToConsole

        /// <summary>
        /// Констрктор
        /// </summary>
        /// <param name="Path">Путь в файлу с данными</param>
        public Repository(string Path, char sep = ',')
        {
            this.path = Path; // Сохранение пути к файлу с данными
            this.index = 0; // текущая позиция для добавления сотрудника в workers
            this.sep = sep;
            this.titles = new string[0]; // инициализаия массива заголовков   
            this.workers = new Worker[1]; // инициализаия массива сотрудников.    | изначально предпологаем, что данных нет

            this.Load(); // Загрузка данных
        }

        /// <summary>
        /// Метод увеличения текущего хранилища
        /// </summary>
        /// <param name="Flag">Условие увеличения</param>
        private void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.workers, this.workers.Length * 2);
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
        /// Метод добавления сотрудника в хранилище
        /// </summary>
        /// <param name="newWorker">Сотрудник</param>
        public void Add(Worker newWorker)
        {
            this.Resize(index >= this.workers.Length);
            this.workers[index] = newWorker;
            this.index++;
        }

        /// <summary>
        /// Метод получения сотрудника по запрашиваемому ID
        /// </summary>
        /// <param name="id">идентификатор сотрудника</param>
        /// <returns>экземпляр класса Worker или null</returns>
        public Worker GetWorkerById(int id)
        {
            // происходит чтение из файла, возвращается Worker
            // с запрашиваемым ID
            for (int i = 0; i < index; i++)
            {
                if (workers[i].ID == id) return workers[i];
            }
            return null;
        }

        /// <summary>
        /// Удаление записи о сотруднике по ID
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        /// <returns>true - найден и удалён, false - ID не найден</returns>
        public bool DeleteWorker(int id)
        {
            bool res = false;
            Worker worker = GetWorkerById(id);
            if (worker != null)
            {
                if (index > 1)
                {
                    Worker[] tmpWorkers = new Worker[index - 1];
                    for (int i = 0, j = 0; i < index; i++)
                    {
                        if (workers[i].ID == worker.ID)
                        {
                            continue;
                        }
                        else
                        {
                            tmpWorkers[j++] = workers[i];
                        }
                    }
                    index--;
                    workers = tmpWorkers;
                }
                else
                {
                    index = 0;
                    workers = new Worker[1];
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Метод загрузки данных
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
                        Add(new Worker(sr.ReadLine(), sep));
                    }
                }
            }
            else
            {
                //Console.WriteLine(outPatern, "ID", "Запись создана", "Ф. И. О.", "Возр", "Рост", "День рожд.", "Место рождения", "Должность");
                titles = new string[8];
                titles[0] = "ID";
                titles[1] = "Запись создана";
                titles[2] = "Ф. И. О.";
                titles[3] = "Возр";
                titles[4] = "Рост";
                titles[5] = "День рожд.";
                titles[6] = "Место рождения";
                titles[7] = "Должность";
            }
        }

        /// <summary>
        /// Метод сохранения данных
        /// </summary>
        /// <param name="Path">Путь к файлу сохранения</param>
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
                    records[i + 1] = workers[i].CsvRecord(sep);
                }
                File.WriteAllLines(Path, records);
            }
        }

        /// <summary>
        /// Получение выборочного списка сотрудников по одному из критериев
        /// </summary>
        /// <param name="mode">тип критерия</param>
        public void GetCustomInfo(string mode)
        {
            Worker[] arr = new Worker[1];
            int currentIndex = 0, i;
            string strInp1, strInp2, strTitle;
            DateTime dt1, dt2, dt3;

            switch(mode)
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
                        dt3 = StringToDateTime(workers[i].DateCreate);
                        if (dt3 > dt1 && dt3 < dt2)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = workers[i];                            
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список сотрудников, принятых с " + strInp1 + " по " + strInp2 + " :";
                        PrintSelectToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nСотрудников с " + strInp1 + " по " + strInp2 + "принято не было\n");
                    }
                    break;
                case "2":   //  записи по должностям
                    Console.WriteLine("Введите должность сотрудников (пример : инженер или бухгалтер)");
                    strInp1 = Console.ReadLine();
                    for (i = 0; i < index; i++)
                    {
                        if (workers[i].Position == strInp1)
                        {
                            if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                            arr[currentIndex++] = workers[i];
                        }
                    }
                    if (currentIndex > 0)
                    {
                        strTitle = "Список сотрудников в должности " + strInp1 + " :";
                        PrintSelectToConsole(arr, strTitle);
                    }
                    else
                    {
                        Console.WriteLine("\nСотрудников в должности " + strInp1 + "нет\n");
                    }
                    break;
                case "3":   //  записи по месяцу дня рождения
                    Console.WriteLine("Введите месяц рождения сотрудников (пример : 01 для января ... 12 для декабря)");
                    strInp1 = Console.ReadLine();
                    if (int.TryParse(strInp1, out int month))
                    {
                        for (i = 0; i < index; i++)
                        {
                            dt1 = StringToDateTime(workers[i].BirthDay);
                            if (dt1.Month == month)
                            {
                                if (currentIndex == arr.Length) Array.Resize(ref arr, arr.Length + 1);
                                arr[currentIndex++] = workers[i];
                            }
                        }
                        if (currentIndex > 0)
                        {
                            strTitle = "Список сотрудников, рождённых в " + strInp1 + " месяце :";
                            PrintSelectToConsole(arr, strTitle);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка в фолмате ввода месяца (пример : 01 для января ... 12 для декабря) => " + strInp1);
                    }
                    break;

            }
        }

        /// <summary>
        /// Вывод переданного массива с данными о сотрудниках
        /// </summary>
        /// <param name="arr">массив с данными о сотрудниках</param>
        /// <param name="title">заголовок для списка</param>
        public void PrintSelectToConsole(Worker[] arr, string title = "Список сотрудников")
        {
            if (arr != null && arr.Length > 0)
            {
                string outPatern = "| {0, -2} | {1, -16} | {2, -30} | {3, -4} | {4, -4} | {5, 10} | {6, -16} | {7, -12} |";
                Console.WriteLine("\n" + title + "\n");
                Console.WriteLine(outPatern, titles[0], titles[1], titles[2], titles[3], titles[4], titles[5], titles[6], titles[7]);

                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine(arr[i].OutFormatString(outPatern));
                }
            }
            else
            {
                Console.WriteLine("\nСписок сотрудников пуст\n");
            }
        }

        /// <summary>
        /// Вывод всех данных в консоль
        /// </summary>
        public void PrintDbToConsole()
        {
            if (index > 0)
            {
                string outPatern = "| {0, -2} | {1, -16} | {2, -30} | {3, -4} | {4, -4} | {5, 10} | {6, -16} | {7, -12} |";
                if (workers.Length > 0)
                {
                    Console.WriteLine("\nСписок сотрудников\n");
                    //Console.WriteLine(outPatern, "ID", "Запись создана", "Ф. И. О.", "Возр", "Рост", "День рожд.", "Место рождения", "Должность");
                    Console.WriteLine(outPatern, titles[0], titles[1], titles[2], titles[3], titles[4], titles[5], titles[6], titles[7]);

                    for (int i = 0; i < index; i++)
                    {
                        Console.WriteLine(workers[i].OutFormatString(outPatern));
                    }
                }                
            }
            else
            {
                Console.WriteLine("\nСписок сотрудников пуст\n");
            }
        }

        /// <summary>
        /// Вывод данных о сотруднике в консоль
        /// </summary>
        /// <param name="worker"></param>
        public void PrintWorker(Worker worker)
        {
            string outPatern = "| {0, -2} | {1, -16} | {2, -30} | {3, -4} | {4, -4} | {5, 10} | {6, -16} | {7, -12} |";
            Console.WriteLine(outPatern, titles[0], titles[1], titles[2], titles[3], titles[4], titles[5], titles[6], titles[7]);
            Console.WriteLine(worker.OutFormatString(outPatern));
        }

        /// <summary>
        /// Количество записей о сотрудниках в хранилище
        /// </summary>
        public int Count { get { return this.index; } }


    }

}
