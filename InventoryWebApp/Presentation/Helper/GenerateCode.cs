using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helper
{
    public static class GenerateCode
    {
        public static string GetOrderCode()
        {
            DatabaseContext db = new DatabaseContext();

            int currentMonth = Convert.ToInt32(GetPersianDate.GetTodayMonthNumber());
            int currentYear = Convert.ToInt32(GetPersianDate.GetTodayYearNumber());

            string result = currentYear + "/" + currentMonth + "/1";

            Order order = db.Orders.Where(current => current.ParentId == null)
                .OrderByDescending(current => current.CreationDate).FirstOrDefault();

            if (order != null)
            {
                string[] inputParts = order.Code.Split('/');

                if (Convert.ToInt32(inputParts[0]) != currentYear)
                {
                    inputParts[0] = currentYear.ToString();
                    inputParts[2] = "0";
                }

                if (Convert.ToInt32(inputParts[1]) != currentMonth)
                {
                    inputParts[1] = currentMonth.ToString();
                    inputParts[2] = "0";
                }

               string day = (Convert.ToInt32(inputParts[2]) + 1).ToString();

                return inputParts[0] + "/" + inputParts[1] + "/" + day;

            }

            return result;
        }

        public static int GetInputCode()
        {
            DatabaseContext db = new DatabaseContext();

           int result = 1;

            Input input = db.Inputs.Where(current => current.IsDeleted == false)
                .OrderByDescending(current => current.CreationDate).FirstOrDefault();

            if (input != null)
                return input.Code + 1;

            return result;
        }
    }
}