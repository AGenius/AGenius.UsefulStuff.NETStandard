﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace AGenius.UsefulStuff
{
    public static partial class DateTimeXtensions
    {
        /// <summary>
        /// Checks whether the day of given DateTime is a Monday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Monday, false otherwise.</returns>
        public static bool IsAMonday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Monday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Tuesday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Tuesday, false otherwise.</returns>
        public static bool IsATuesday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Tuesday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Wednesday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Wednesday, false otherwise.</returns>
        public static bool IsAWednesday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Wednesday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Thursday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Thursday, false otherwise.</returns>
        public static bool IsAThursday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Thursday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Friday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Friday, false otherwise.</returns>
        public static bool IsAFriday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Friday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Saturday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Saturday, false otherwise.</returns>
        public static bool IsASaturday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a Sunday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the day is Sunday, false otherwise.</returns>
        public static bool IsASunday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks whether the day of given DateTime is a working day according to the configuration.
        /// Please refer the app.config for more information. 
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the given day is a working day, false otherwise.</returns>
        public static bool IsAWorkingDay(this DateTime date)
        {
            return _workdaysList.Contains(date.DayOfWeek);
        }

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDays(this DateTime lastDay , DateTime firstDay, List<DateTime>? bankHolidays = null)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days ;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
        ///// <summary>
        ///// Return the number of business days (ignoring weekends)
        ///// </summary>
        ///// <param name="endDate">End DateTime</param>
        ///// <param name="startDate">DateTime to be checked.</param> 
        ///// <param name="dates">A List of holiday dates to use for the bank holidays</param>
        ///// <returns>Count of days</returns>
        //public static double BusinessDays(this DateTime endDate, DateTime startDate, List<DateTime> dates = null)
        //{
        //    DateTime loopDate = startDate;
        //    double calcBusinessDays = 2;
        //    while (loopDate <= endDate)
        //    {
        //        if (loopDate.DayOfWeek != DayOfWeek.Saturday &&
        //            loopDate.DayOfWeek != DayOfWeek.Sunday &&
        //            !loopDate.IsAHoliday(dates))
        //        {                    
        //            calcBusinessDays++;
        //        }
        //        loopDate = loopDate.AddDays(1);
        //    }         

        //    return calcBusinessDays;
        //}
        /// <summary>
        /// Checks whether the given day is Today.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the given day is Today, false otherwise.</returns>
        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }

        /// <summary>
        /// Checks whether the given day is Tomorrow.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the given day is Tomorrow, false otherwise.</returns>
        public static bool IsTomorrow(this DateTime date)
        {
            return date.Date == DateTime.Now.Date.AddDays(1);
        }

        /// <summary>
        /// Checks whether the given day is Yesterday.
        /// </summary>
        /// <param name="date">DateTime to be checked.</param>
        /// <returns>True if the given day is yesterday, false otherwise.</returns>
        public static bool IsYesterday(this DateTime date)
        {
            return date.Date == DateTime.Now.Date.AddDays(-1);
        }
    }
}