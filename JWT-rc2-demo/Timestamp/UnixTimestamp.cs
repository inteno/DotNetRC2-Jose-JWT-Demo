using System;

namespace HelloMvc.Timestamp
{
    public class UnixTimestamp
    {
        private static DateTime UnixStartDate = new DateTime(1970, 1, 1, 0, 0, 0);

        public static int Now()
        {
            return UnixTimestamp.ConvertToTimestamp(DateTime.UtcNow);
        }
        public static int ConvertToTimestamp(DateTime date)
        {
            TimeSpan ts = date - UnixStartDate;
            return (int)ts.TotalSeconds;
        }
        public static DateTime ConvertToDate(string timestampString)
        {
            if (String.IsNullOrWhiteSpace(timestampString)) {
                throw new Exception("Empty timestamp");
            }
            return ConvertToDate(int.Parse(timestampString));
        }
        public static DateTime ConvertToDate(int timestamp)
        {
            return UnixStartDate.AddSeconds(timestamp);
        }
    }
}