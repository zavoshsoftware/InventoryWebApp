using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TempCustomer
    {
        [Key]
        public int Id { get; set; }
        public string Buyer_Code { get; set; }
        public string Buyer_Name { get; set; }
        public string Buyer_FirstName { get; set; }
        public string Buyer_LastName { get; set; }
        public string Buyer_Address { get; set; }
        public string Buyer_Tel { get; set; }
        public string Buyer_Mobile { get; set; }
        public string Buyer_Fax { get; set; }
        public bool Buyer_Bank { get; set; }
        public bool Bussiness { get; set; }
        public string Buyer_Type { get; set; }
        public string Pass_Date { get; set; }
        public decimal Remind_Cash { get; set; }
        public bool Is_Shown { get; set; }
        public string Buyer_Include { get; set; }

    }
}
