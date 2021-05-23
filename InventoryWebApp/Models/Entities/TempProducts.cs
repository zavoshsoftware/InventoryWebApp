using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class TempProduct
    {
        [Key]
        public int Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Product_Name1 { get; set; }
        public int Product_Type_Code { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public string Condition { get; set; }
        public string Grade { get; set; }
        public string Maker { get; set; }
        public string Type { get; set; }
        public string Other { get; set; }
        public bool Net_weight { get; set; }
        public Int64 App_Weight { get; set; }
    }
}
