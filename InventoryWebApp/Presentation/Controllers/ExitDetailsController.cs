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
    public class ExitDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index(Guid id)
        {
            List<ExitDetail> exitDetails = UnitOfWork.ExitDetailRepository.Get(e => e.ExitId == id)
                .OrderByDescending(e => e.CreationDate).ToList();

            Exit exit = UnitOfWork.ExitRepository.GetById(id);

            ExitDetailViewModel result = new ExitDetailViewModel()
            {
                Exit = exit,
                ExitDetails = exitDetails
            };

            return View(result);
        }

        public ActionResult Edit(string exitDetailId, string fullWeight, string emptyWeight)
        {
            try
            {


                Guid id = new Guid(exitDetailId);

                ExitDetail exitDetail = UnitOfWork.ExitDetailRepository.GetById(id);

                exitDetail.FullWeight = Convert.ToDecimal(fullWeight);
                exitDetail.EmptyWeight = Convert.ToDecimal(emptyWeight);
                exitDetail.PureWeight = Convert.ToDecimal(fullWeight) - Convert.ToDecimal(emptyWeight);

                UnitOfWork.ExitDetailRepository.Update(exitDetail);
                UnitOfWork.Save();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CalculateAccounts(string exitId)
        {
            try
            {
                Guid id = new Guid(exitId);

                List<ExitDetail> exitDetails = UnitOfWork.ExitDetailRepository.Get(c => c.ExitId == id).ToList();

                decimal amount = 0;
                foreach (ExitDetail exitDetail in exitDetails)
                {
                    if (exitDetail.PureWeight <= 1000)
                        amount += exitDetail.InputDetail.Product.ProductGroup.InventoryAmount;

                    else
                    {
                        int ton = (int)Math.Ceiling((double)exitDetail.PureWeight / 1000);

                        amount += (exitDetail.InputDetail.Product.ProductGroup.InventoryAmount) * ton;
                    }
                }
                decimal vat = (amount + 60000) * 0.09m;
                decimal total = (amount + 60000) + vat;

                ExitAccountViewModel result = new ExitAccountViewModel()
                {
                    InventoryAmount = amount.ToString("N0"),
                    CutAmount = "0",
                    LoadAmount = "60,000",
                    OtherAmount = "0",
                    WeighbridgeAmount = "0",
                    Vat = vat.ToString("N0"),
                    TotalAmount = total.ToString("N0"),

                };



                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalizeExit(string exitId, string cutAmount, string otherAmount, string weighbridgeAmount,
                                         string loadAmount, string inventoryAmount)
        {
            try
            {
                Guid id = new Guid(exitId);

                Exit exit = UnitOfWork.ExitRepository.GetById(id);

                decimal subTotalAmount = Convert.ToDecimal(inventoryAmount) + Convert.ToDecimal(loadAmount) +
                                      Convert.ToDecimal(cutAmount) +
                                      Convert.ToDecimal(otherAmount) + Convert.ToDecimal(weighbridgeAmount);

                decimal vat = subTotalAmount * 0.09m;

                decimal totalAmount = vat + subTotalAmount;

                exit.InventoryAmount = Convert.ToDecimal(inventoryAmount);
                exit.LoadAmount = Convert.ToDecimal(loadAmount);
                exit.CutAmount = Convert.ToDecimal(cutAmount);
                exit.OtherAmount = Convert.ToDecimal(otherAmount);
                exit.WeighbridgeAmount = Convert.ToDecimal(weighbridgeAmount);
                exit.SubTotalAmount = subTotalAmount;
                exit.Vat = vat;
                exit.TotalAmount = totalAmount;

                exit.ExitComplete = true;

                UnitOfWork.ExitRepository.Update(exit);
                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
