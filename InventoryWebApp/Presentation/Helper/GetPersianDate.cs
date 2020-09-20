using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helper
{
    public static class GetPersianDate
    {
        public static string GetTodayMonthNumber()
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string month = pc.GetMonth(DateTime.Today).ToString().PadLeft(2, '0');
            return month;
        }

        public static string GetTodayYearNumber()
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string year = pc.GetYear(DateTime.Today).ToString().PadLeft(4, '0').Substring(2,2);
            return year;
        }
    }
}