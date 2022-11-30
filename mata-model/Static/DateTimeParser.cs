using System;

namespace DataModel.Static
{
    public static class DateTimeParser
    {
        // https://stackoverflow.com/questions/919244/converting-a-string-to-datetime
        public static DateTime ToDateTime(this string datetime, char dateSpliter = '-', char timeSpliter = ':', char millisecondSpliter = ',')
        {
            try
            {
                datetime = datetime.Trim();
                datetime = datetime.Replace("  ", " ");
                string[] body = datetime.Split(' ');
                string[] date = body[0].Split(dateSpliter);
                int year = date[0].ToInt();
                int month = date[1].ToInt();
                int day = date[2].ToInt();
                int hour = 0, minute = 0, second = 0, millisecond = 0;
                if (body.Length == 2)
                {
                    string[] tpart = body[1].Split(millisecondSpliter);
                    string[] time = tpart[0].Split(timeSpliter);
                    hour = time[0].ToInt();
                    minute = time[1].ToInt();
                    if (time.Length == 3) second = time[2].ToInt();
                    if (tpart.Length == 2) millisecond = tpart[1].ToInt();
                }
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch
            {
                return new DateTime();
            }
        }

        public static int ToInt(this string stringInt)
        {
            try
            {
                int result = int.Parse(stringInt);
                return result;
            }
            catch { return 0; }
        }
    }
}
