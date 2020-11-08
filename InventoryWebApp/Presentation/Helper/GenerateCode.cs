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
    public static string GetChildOrderCode(Guid parentOrderId)
        {
            DatabaseContext db = new DatabaseContext();

            Order order = db.Orders.Find(parentOrderId);

            Order lastOrderChild = db.Orders.Where(c => c.ParentId == parentOrderId && c.IsDeleted == false)
                .OrderByDescending(c => c.Code).FirstOrDefault();

            if (lastOrderChild == null)
                return order.Code + "/1";

            else
            {
                string[] orderCodeitems = lastOrderChild.Code.Split('/');
                int newCode = Convert.ToInt32(orderCodeitems.LastOrDefault()) + 1;

                return order.Code + "/" + newCode;
            }

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
        //public static string GetInputDetailCode(Guid orderId)
        //{
        //    DatabaseContext db = new DatabaseContext();

        //   string result = "1";

        //    InputDetail inputDetail = db.InputDetails.Where(current =>current.OrderId==orderId&& current.IsDeleted == false)
        //        .OrderByDescending(current => current.CreationDate).FirstOrDefault();

        //    if (inputDetail != null)
        //        return (Convert.ToInt32(inputDetail.Code) + 1).ToString();

        //    return result;
        //}

        public static int GetExitCode()
        {
            DatabaseContext db = new DatabaseContext();

            int result = 1;

            Exit exit = db.Exits.Where(current => current.IsDeleted == false)
                .OrderByDescending(current => current.CreationDate).FirstOrDefault();

            if (exit != null)
                return exit.Code + 1;

            return result;
        }
    }
}