using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class ExitArchiveController : Infrastructure.BaseControllerWithUnitOfWork
    {
       

        // GET: ExitArchive
        public ActionResult Index()
        {
            var exits = UnitOfWork.ExitRepository.Get(current=>current.ExitComplete==true).OrderByDescending(e=>e.CreationDate);
            return View(exits.ToList());
        }

    
        public ActionResult Details(Guid id)
        {
            List<ExitDetail> exitDetails = UnitOfWork.ExitDetailRepository.Get(e => e.ExitId == id)
                .OrderByDescending(e => e.CreationDate).ToList();

            Exit exit = UnitOfWork.ExitRepository.GetById(id);

            ExitDetailViewModel result = new ExitDetailViewModel()
            {
                Exit = exit,
                ExitDetails = exitDetails
            };
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");

            return View(result);
        }

       
    }
}
