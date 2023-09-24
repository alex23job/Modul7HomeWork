using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_ConsoleApp
{
    class Program
    {
        static string fileName = "data.csv";
        static void Main(string[] args)
        {
            string strInput = "";
            Repository repos = new Repository(fileName, '#');
            Console.WriteLine("Справочник\"Сотрудник\"");
            while (strInput != "0")
            {
                Console.WriteLine("\nДля вывода справочника нажмите < 1 >");
                Console.WriteLine("Для ввода информации о новом сотруднике нажмите < 2 >");
                Console.WriteLine("Для вывода информации о сотруднике по ID нажмите < 3 >");
                Console.WriteLine("Для удаления информации о сотруднике по ID нажмите < 4 >");
                Console.WriteLine("Для просмотра отсортированной по некоторым критериям\nинформации о сотрудниках нажмите < 5 >");
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
                        GetInfoByID(repos);
                        break;
                    case "4":
                        DeleteWorkerByID(repos);
                        break;
                    case "5":
                        CustomInfo(repos);
                        break;
                    default:
                        Console.WriteLine("Ошибка при вводе команды выбора действия => " + strInput);
                        break;
                }
            }
        }

        private static void CustomInfo(Repository repos)
        {
            string strInput = "";
            while(strInput != "0")
            {
                Console.WriteLine("\nПросмотр выборочной информации: \n");
                Console.WriteLine("Для просмотра записей в выбранном диапазоне дат нажмите  < 1 >");
                Console.WriteLine("Для просмотра записей о сотрудниках по должности нажмите  < 2 >");
                Console.WriteLine("Для просмотра записей по месяцу дня рождения нажмите  < 3 >");
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
                    default:
                        Console.WriteLine("Ошибка при вводе команды выбора действия => " + strInput);
                        break;
                }
            }
        }

        private static void DeleteWorkerByID(Repository rep)
        {
            Console.WriteLine("\nУдаление данных сотрудника по его идентификатору: \n");
            Console.WriteLine("Введите идентификатор сотрудника :");
            string strID = Console.ReadLine();
            if (int.TryParse(strID, out int id))
            {
                Worker worker = rep.GetWorkerById(id);
                if (worker != null)
                {
                    Console.WriteLine("Будет удалена инфомация о струднике");
                    rep.PrintWorker(worker);
                    rep.DeleteWorker(id);
                }
                else
                {
                    Console.WriteLine("Сотрудника с ID = " + id + " в базе нет.");
                }
            }
        }

        static void PrintList()
        {
            string[] records = GetAllRecords(fileName);
            if (records != null)
            {
                string outPatern = "| {0, -2} | {1, -16} | {2, -30} | {3, -4} | {4, -4} | {5, 10} | {6, -16} | {7, -12} |";
                if (records.Length > 0)
                {
                    Console.WriteLine("\nСписок сотрудников\n");
                    Console.WriteLine(outPatern, "ID", "Запись создана", "Ф. И. О.", "Возр", "Рост", "День рожд.", "Место рождения", "Должность");
                    foreach (string rec in records)
                    {
                        Worker wp = new Worker(rec, '#');
                        Console.WriteLine(wp.OutFormatString(outPatern));
                    }
                }
            }
        }

        static void InputInfo(Repository rep)
        {
            Worker wp = new Worker();
            wp.ID = rep.Count + 1;

            Console.WriteLine("\nВвод данных нового сотрудника : \n");
            Console.WriteLine("Введите фамилию, имя, отчество сотрудника (пример : Иванов Иван Иванович)");
            wp.FullName = Console.ReadLine();
            Console.WriteLine("Введите возраст сотрудника :");
            string strAge = Console.ReadLine();
            if (int.TryParse(strAge, out int age))
            {
                wp.Age = age;
            }
            Console.WriteLine("Введите рост сотрудника :");
            string strHeight = Console.ReadLine();
            if (int.TryParse(strHeight, out int h))
            {
                wp.Height = h;
            }
            Console.WriteLine("Введите дату рождения сотрудника (пример : 01.01.1970)");
            wp.BirthDay = Console.ReadLine();
            Console.WriteLine("Введите место рождения сотрудника (пример : город Тула)");
            wp.BirthCity = Console.ReadLine();
            Console.WriteLine("Введите должность сотрудника (пример : стажёр)");
            wp.Position = Console.ReadLine();

            rep.Add(wp);
        }

        static void GetInfoByID(Repository rep)
        {
            Console.WriteLine("\nПолучение данных сотрудника по его идентификатору: \n");
            Console.WriteLine("Введите идентификатор сотрудника :");
            string strID = Console.ReadLine();
            if (int.TryParse(strID, out int id))
            {
                Worker worker = rep.GetWorkerById(id);
                if (worker != null)
                {
                    rep.PrintWorker(worker);
                }
                else
                {
                    Console.WriteLine("Сотрудника с ID = " + id + " в базе нет.");
                }
            }
        }

        static string[] GetAllRecords(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllLines(fileName);
            }
            return null;
        }
    }
}
