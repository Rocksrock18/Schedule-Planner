using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulePlanner.Static_Factories
{
    /// <summary>
    /// Class for converting various types of data to other data types.
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// Converts string representation of date to corresponding integer representation.
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <returns>Integer representation of date</returns>
        public static int String_To_Date(string date)
        {
            int year = Int32.Parse(date.Substring(date.Length - 4));
            int divider = date.IndexOf("/");
            string temp_day = date.Substring(divider + 1, 2);
            if (temp_day[1] == '/')
            {
                temp_day = temp_day.Substring(0, 1);
            }
            int day = int.Parse(temp_day);
            int month = int.Parse(date.Substring(0, divider));
            return day + month * 100 + year * 10000;
        }

        /// <summary>
        /// Converts integer representation of date to corresponding DateTime.
        /// </summary>
        /// <param name="date">integer representation of date</param>
        /// <returns>DateTime representation of date</returns>
        public static DateTime Date_To_Date_Time(int date)
        {
            int day = date % 100;
            date /= 100;
            int month = date % 100;
            date /= 100;
            int year = date;
            return new DateTime(year, month, day);
        }
    }
}
