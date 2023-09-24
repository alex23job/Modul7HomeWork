using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayPlanner_ConsoleApp
{
    class Program
    {
        static string fileName = "planner.csv";
        static void Main(string[] args)
        {
            string strInput = "";
            Repository repos = new Repository(fileName, '#');
            Console.WriteLine("Планировщик мероприятий \"Ежедневник\"");
            while (strInput != "0")
            {
                Console.WriteLine("\nДля вывода списка мероприятий нажмите < 1 >");
                Console.WriteLine("Для ввода информации о новом мероприятии нажмите < 2 >");
                Console.WriteLine("Для редактирования информации о мероприятии по ID нажмите < 3 >");
                Console.WriteLine("Для удаления информации о мероприятии по ID нажмите < 4 >");
                Console.WriteLine("Для просмотра отсортированной по некоторым критериям\nинформации о мероприятиях нажмите < 5 >");
                Console.WriteLine("Для добавления списка мероприятий из файла нажмите < 6 >");
                Console.WriteLine("Для ввода имени файла и записи списка мероприятий в него нажмите < 7 >");
                Console.WriteLine("Для завершения нажмите < 0 >");
                strInput = Console.ReadLine();
                switch (strInput)
                {
                    case "0":
                        repos.Save(fileName);
                        break;
                    case "1":
                        repos.PrintDbToConsole();
                        break;
                    case "2":
                        InputInfo(repos);
                        break;
                    case "3":
                        EditInfoByID(repos);
                        break;
                    case "4":
                        DeleteEntryByID(repos);
                        break;
                    case "5":
                        CustomInfo(repos);
                        break;
                    case "6":
                        AddListEntrys(repos);
                        break;
                    case "7":
                        SaveAs(repos);
                        break;
                    default:
                        Console.WriteLine("Ошибка при вводе команды выбора действия => " + strInput);
                        break;
                }
            }
        }

        /// <summary>
        /// Получение по некоторым критериям информации о мероприятиях
        /// </summary>
        /// <param name="repos">экземпляр класса Repository</param>
        private static void CustomInfo(Repository repos)
        {
            string strInput = "";
            while (strInput != "0")
            {
                Console.WriteLine("\nПросмотр выборочной информации: \n");
                Console.WriteLine("Для просмотра мероприятий в выбранном диапазоне дат нажмите  < 1 >");
                Console.WriteLine("Для просмотра мероприятий по месту провеления нажмите  < 2 >");
                Console.WriteLine("Для просмотра мероприятий по наименованию нажмите  < 3 >");
                Console.WriteLine("Для просмотра не выполненных мероприятий нажмите  < 4 >");
                Console.WriteLine("Для выхода в главное меню нажмите < 0 >");
                strInput = Console.ReadLine();
                switch (strInput)
                {
                    case "0":
                        break;
                    case "1":
                        repos.GetCustomInfo(strInput);
                        break;
                    case "2":
                        repos.GetCustomInfo(strInput);
                        break;
                    case "3":
                        repos.GetCustomInfo(strInput);
                        break;
                    case "4":
                        repos.GetCustomInfo(strInput);
                        break;
                    default:
                        Console.WriteLine("Ошибка при вводе команды выбора действия => " + strInput);
                        break;
                }
            }
        }

        /// <summary>
        /// Добавление в список мероприятий из файла
        /// </summary>
        /// <param name="repos">экземпляр класса Repository</param>
        private static void AddListEntrys(Repository repos)
        {
            Console.WriteLine("Введите имя файла со списком мероприятий для добавления : ");
            string fileName = Console.ReadLine();
            repos.AddListFromFile(fileName);
        }

        /// <summary>
        /// Сохранение списка мероприятий в другой файл
        /// </summary>
        /// <param name="repos">экземпляр класса Repository</param>
        private static void SaveAs(Repository repos)
        {
            Console.WriteLine("Введите имя файла для сохранения списка мероприятий : ");
            string fileName = Console.ReadLine();
            repos.Save(fileName);
        }

        /// <summary>
        /// Выбор мероприятия и редактирование информации о нем
        /// </summary>
        /// <param name="repos">экземпляр класса Repository</param>
        private static void EditInfoByID(Repository repos)
        {
            string strInput = "";
            int id = -1;
            string strReturn = "";

            Console.WriteLine("\nРедактирование информации о мероприятии : \n");
            while (strInput != "0")
            {
                Console.WriteLine("\nДля ввода идентификатора мероприятия нажмите < 1 >");
                Console.WriteLine("Для редактирования даты мероприятия нажмите < 2 >");
                Console.WriteLine("Для редактирования времени мероприятия нажмите < 3 >");
                Console.WriteLine("Для редактирования места мероприятия нажмите < 4 >");
                Console.WriteLine("Для редактирования наименования мероприятия нажмите < 5 >");
                Console.WriteLine("Для редактирования продолжительности мероприятия нажмите < 6 >");
                Console.WriteLine("Для редактирования отметки о выполнении мероприятия нажмите < 7 >");
                Console.WriteLine("Для редактирования примечания о мероприятии нажмите < 8 >");
                Console.WriteLine("Для завершения редактирования нажмите < 0 >");
                strInput = Console.ReadLine();
                switch (strInput)
                {
                    case "0":
                        break;
                    case "1":
                        Console.WriteLine("Введите идентификатор мероприятия :");
                        strInput = Console.ReadLine();
                        if (int.TryParse(strInput, out id) == false)
                        {
                            Console.WriteLine("Ошибка ввода идентификатора. Должно быть число а не " + strInput);
                        }
                        break;
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (id != -1)
                        {
                            strReturn = repos.Edit(id, strInput);
                            if (strReturn != "")
                            {
                                Console.WriteLine(strReturn);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не был введён идентификатор мероприятия ...");
                        }
                        break;
                    default:
                        Console.WriteLine("Ошибка при вводе команды выбора действия => " + strInput);
                        break;
                }
            }
        }

        /// <summary>
        /// Заполнение информацмм для добавления новой записи
        /// </summary>
        /// <param name="rep">экземпляр класса Repository</param>
        static void InputInfo(Repository rep)
        {
            MyEntry me = new MyEntry();
            me.ID = rep.Count + 1;

            Console.WriteLine("\nВвод данных нового мероприятия : \n");
            Console.WriteLine("Введите дату мероприятия (пример : 01.01.1970 )");
            me.Date = Console.ReadLine();
            Console.WriteLine("Введите время начала мероприятия (пример : 14:30 )");
            me.Time = Console.ReadLine();
            Console.WriteLine("Введите место проведения мероприятия (пример : кабинет )");
            me.Place = Console.ReadLine();
            Console.WriteLine("Введите наименование мероприятия (пример : совещание )");
            me.EventName = Console.ReadLine();
            Console.WriteLine("Введите продолжительность мероприятия в минутах (пример : 30 )");
            me.Duration = Console.ReadLine();
            Console.WriteLine("Введите примечание (пример : - )");
            me.Remark = Console.ReadLine();

            rep.Add(me);
        }

        /// <summary>
        /// Удаление мероприятия по его идентификатору
        /// </summary>
        /// <param name="repos">экземпляр класса Repository</param>
        private static void DeleteEntryByID(Repository repos)
        {
            Console.WriteLine("\nУдаление мероприятия по его идентификатору: \n");
            Console.WriteLine("Введите идентификатор мероприятия :");
            string strID = Console.ReadLine();
            if (int.TryParse(strID, out int id))
            {
                MyEntry me = repos.GetEntryById(id);
                if (me != null)
                {
                    Console.WriteLine("Будет удалена инфомация о мероприятии");
                    Console.WriteLine(me.ToString());
                    repos.DeleteEntry(id);
                }
                else
                {
                    Console.WriteLine("Мероприятия с ID = " + id + " в базе нет.");
                }
            }
        }
    }
}
