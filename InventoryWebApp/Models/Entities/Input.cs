﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Code { get; set; }

        [Display(Name = "InputDate", ResourceType = typeof(Resources.Models.Input))]
        public DateTime InputDate { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "PostRentAmount", ResourceType = typeof(Resources.Models.Input))] public string InputTime { get; set; }
        public decimal? PostRentAmount { get; set; }

        [Display(Name = "OtherAmount", ResourceType = typeof(Resources.Models.Input))] public decimal? WeighbridgeAmount { get; set; }
        public decimal? OtherAmount { get; set; }

        [Display(Name = "SourceWeight", ResourceType = typeof(Resources.Models.Input))] public decimal? CommissionAmount { get; set; }
        public decimal? SourceWeight { get; set; }

        [Display(Name = "FullWeight", ResourceType = typeof(Resources.Models.Input))] public decimal DestinationWeight { get; set; }
        public decimal? FullWeight { get; set; }

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
        public string InputDesc { get; set; }


        public virtual ICollection<InputDetail> InputDetails { get; set; }
    }
}