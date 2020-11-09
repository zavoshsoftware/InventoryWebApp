using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
namespace ViewModels
{
    public class ExitDetailViewModel
    {
        public Exit Exit { get; set; }
        public List<ExitDetail> ExitDetails { get; set; }
    }
}