using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_ConsoleApp
{
    class Worker
    {
        /// <summary>
        /// Идентификатор-
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата создания записи (приёма на работу)
        /// </summary>
        public string DateCreate { get; set; }

        /// <summary>
        /// Ф. И. О.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Возраст сотрудника
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Рост сотрудника
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// город рождения
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Worker()
        {
            ID = 0;
            DateTime dt = DateTime.Now;
            DateCreate = string.Format("{0:D02}.{1:D02}.{2:D04} {3:D02}:{4:D02}", dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute);
            FullName = "";
            Age = 0;
            Height = 0;
            BirthDay = "";
            BirthCity = "";
            Position = "";
        }

        /// <summary>
        /// Конструктор для получения записи из строки в CSV формате
        /// </summary>
        /// <param name="csvStr">строка в CSV формате</param>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        public Worker(string csvStr, char sep = ';')
        {
            string[] zn = csvStr.Split(sep);
            if (zn.Length >= 8)
            {
                if (int.TryParse(zn[0], out int id))
                {
                    ID = id;
                }
                DateCreate = zn[1];
                FullName = zn[2];
                if (int.TryParse(zn[3], out int age))
                {
                    Age = age;
                }
                if (int.TryParse(zn[4], out int height))
                {
                    Height = height;
                }
                BirthDay = zn[5];
                BirthCity = zn[6];
                Position = zn[7];
            }
        }

        /// <summary>
        /// Конструктор для заполнения всех полей экземпляра класса
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fn"></param>
        /// <param name="age"></param>
        /// <param name="height"></param>
        /// <param name="bd"></param>
        /// <param name="bc"></param>
        /// <param name="pos"></param>
        public Worker(int id, string fn, int age, int height, string bd, string bc, string pos)
        {
            ID = id;
            DateTime dt = DateTime.Now;
            DateCreate = string.Format("{0:D02}.{1:D02}.{2:D04} {3:D02}:{4:D02}", dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute);
            FullName = fn;
            Age = age;
            Height = height;
            BirthDay = bd;
            BirthCity = bc;
            Position = pos;
        }

        /// <summary>
        ///  Формирование строки в CSV формате
        /// </summary>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        /// <returns>строка в CSV формате</returns>
        public string CsvRecord(char sep = ';')
        {
            StringBuilder res = new StringBuilder(ID.ToString());
            res.Append(sep + DateCreate);
            res.Append(sep + FullName);
            res.Append(sep + Age.ToString());
            res.Append(sep + Height.ToString());
            res.Append(sep + BirthDay);
            res.Append(sep + BirthCity);
            res.Append(sep + Position);
            return res.ToString();
        }

        /// <summary>
        /// Формирование строки в задаваемом формате
        /// </summary>
        /// <param name="pattern">формат строки</param>
        /// <returns></returns>
        public string OutFormatString(string pattern)
        {
            return string.Format(pattern, ID, DateCreate, FullName, Age, Height, BirthDay, BirthCity, Position);
        }

        /// <summary>
        /// Формирование строки из некоторых наиболее важных полей экхемпляра класса
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder res = new StringBuilder("Сотрудник № " + ID.ToString());
            res.Append(" " + FullName);
            res.Append(" возраст: " + Age.ToString());
            res.Append(" должность: " + Position);
            return res.ToString();
        }
    }
}
