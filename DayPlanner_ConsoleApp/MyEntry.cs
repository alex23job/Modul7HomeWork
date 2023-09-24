using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayPlanner_ConsoleApp
{
    class MyEntry
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// дата мероприятия
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// время мероприятия
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// место проведения
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// наименование мероприятия
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// продолжительность мероприятия
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// отметка о выполнении
        /// </summary>
        public string Completion { get; set; }

        /// <summary>
        /// примечание
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MyEntry()
        {
            ID = 0;
            Date = "";
            Time = "";
            Place = "";
            EventName = "";
            Duration = "";
            Completion = "нет";
            Remark = "";
        }

        /// <summary>
        /// Конструктор для получения записи из строки в CSV формате
        /// </summary>
        /// <param name="csvStr">строка в CSV формате</param>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        public MyEntry(string csvStr, char sep = ';')
        {
            string[] zn = csvStr.Split(sep);
            if (zn.Length >= 8)
            {
                if (int.TryParse(zn[0], out int id))
                {
                    ID = id;
                }
                Date = zn[1];
                Time = zn[2];
                Place = zn[3];
                EventName = zn[4];
                Duration = zn[5];
                Completion = zn[6];
                Remark = zn[7];
            }
        }

        /// <summary>
        /// Конструктор для заполнения всех полей экземпляра класса
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="dt">дата</param>
        /// <param name="tm">время</param>
        /// <param name="pl">место</param>
        /// <param name="ev">наименование</param>
        /// <param name="dur">продолжительность</param>
        /// <param name="cmp">отметка о выполнении</param>
        /// <param name="rem">примечание</param>
        public MyEntry(int id, string dt, string tm, string pl, string ev, string dur, string cmp, string rem)
        {
            ID = id;
            Date = dt;
            Time = tm;
            Place = pl;
            EventName = ev;
            Duration = dur;
            Completion = cmp;
            Remark = rem;
        }

        /// <summary>
        /// Формирование строки в CSV формате
        /// </summary>
        /// <param name="sep">раздклитель строки в CSV формате</param>
        /// <returns>строка в CSV формате</returns>
        public string CsvRecord(char sep = ';')
        {
            StringBuilder res = new StringBuilder(ID.ToString());
            res.Append(sep + Date);
            res.Append(sep + Time);
            res.Append(sep + Place);
            res.Append(sep + EventName);
            res.Append(sep + Duration);
            res.Append(sep + Completion);
            res.Append(sep + Remark);
            return res.ToString();
        }

        /// <summary>
        /// Получение объекта DateTime по строковым значениям полей даты и времени
        /// </summary>
        /// <returns>объект DateTime или его минимальное значение при ошибках преобразования</returns>
        public DateTime GetDateTime()
        {
            string[] dat = Date.Split('.');
            string[] tm = Time.Split(':');
            if ((dat.Length == 3) && (tm.Length == 2))
            {
                if (int.TryParse(dat[0], out int day) && int.TryParse(dat[1], out int month) && int.TryParse(dat[2], out int year) && int.TryParse(tm[0], out int hour) && int.TryParse(tm[1], out int minute))
                {
                    DateTime dt = new DateTime(year, month, day, hour, minute, 0);
                    return dt;
                }
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Формирование строки в задаваемом формате
        /// </summary>
        /// <param name="pattern">формат строки</param>
        /// <returns></returns>
        public string OutFormatString(string pattern)
        {
            return string.Format(pattern, ID, Date, Time, Place, EventName, Duration, Completion, Remark);
        }

        /// <summary>
        /// Формирование строки из некоторых наиболее важных полей экхемпляра класса
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder res = new StringBuilder("Запись № " + ID.ToString());
            res.Append(" " + Date);
            res.Append(" " + Time);
            res.Append(" " + Place);
            res.Append(" " + EventName);
            res.Append(" вып:" + Completion);
            return res.ToString();
        }
    }

    // сравнение по дате и времени
    class CustomMyEntryComparer : IComparer<MyEntry>
    {
        int IComparer<MyEntry>.Compare(MyEntry x, MyEntry y)
        {
            long xTick = x.GetDateTime().Ticks;
            long yTick = y.GetDateTime().Ticks;
            return (int)((xTick - yTick) / 10000000);
        }
    }
}
