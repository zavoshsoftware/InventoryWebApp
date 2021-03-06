﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Input:BaseEntity
    {
        public Input()
        {
            InputDetails=new List<InputDetail>();
        }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Input))]
        public int Code { get; set; }

        [Display(Name = "InputDate", ResourceType = typeof(Resources.Models.Input))]
        [UIHint("PersianDatePicker")]
        public DateTime InputDate { get; set; }

        [Display(Name = "InputDate", ResourceType = typeof(Resources.Models.Input))]
        [NotMapped]
        public string InputDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(InputDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(InputDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(InputDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }


        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "PostRentAmount", ResourceType = typeof(Resources.Models.Input))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? PostRentAmount { get; set; }

        [Display(Name = "InputTime", ResourceType = typeof(Resources.Models.Input))]
        public string InputTime { get; set; }

        [Display(Name = "OtherAmount", ResourceType = typeof(Resources.Models.Input))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? OtherAmount { get; set; }
        [Display(Name = "WeighbridgeAmount", ResourceType = typeof(Resources.Models.Input))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? WeighbridgeAmount { get; set; }

        [Display(Name = "SourceWeight", ResourceType = typeof(Resources.Models.Input))]
        public decimal? SourceWeight { get; set; }

        [Display(Name = "CommissionAmount", ResourceType = typeof(Resources.Models.Input))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? CommissionAmount { get; set; }

        [Display(Name = "FullWeight", ResourceType = typeof(Resources.Models.Input))] 
        public decimal? FullWeight { get; set; }

        [Display(Name = "DestinationWeight", ResourceType = typeof(Resources.Models.Input))]
        public decimal DestinationWeight { get; set; }

        [Display(Name = "EmptyWeight", ResourceType = typeof(Resources.Models.Input))]
        public decimal? EmptyWeight { get; set; }

        [Display(Name = "TransporterCode", ResourceType = typeof(Resources.Models.Input))]
        public string TransporterCode { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Transporter))]
        public Guid TransporterId { get; set; }
        public virtual Transporter Transporter { get; set; }

        [Display(Name = "CityId", ResourceType = typeof(Resources.Models.Input))]
        public Guid CityId { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "CarNumber", ResourceType = typeof(Resources.Models.Input))]
        public string CarNumber { get; set; }

        [Display(Name = "InputDesc", ResourceType = typeof(Resources.Models.Input))]
        [DataType(DataType.MultilineText)]
        public string InputDesc { get; set; }

        public virtual ICollection<InputDetail> InputDetails { get; set; }


        //[Display(Name = "Code", ResourceType = typeof(Resources.Models.Order))]
        //public Guid? OrderId { get; set; }
        //public virtual Order Order { get; set; }


        //internal class configuration : EntityTypeConfiguration<Input>
        //{
        //    public configuration()
        //    {
        //        HasOptional(p => p.Order).WithMany(t => t.Inputs).HasForeignKey(p => p.OrderId);
        //    }
        //}
    }
}
