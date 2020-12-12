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
using System.Reflection;
using System.ComponentModel.DataAnnotations;

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
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");

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
                ManageConfiguration configuration = UnitOfWork.ManageConfigurationRepository.Get().FirstOrDefault();
                decimal amount = 0;
                decimal loadAmount = 0;
                decimal CutAmount = 0;
                decimal OtherAmount = 0;
                decimal WeighbridgeAmount = 0;
                foreach (ExitDetail exitDetail in exitDetails)
                {
                    loadAmount += Convert.ToDecimal(exitDetail.PureWeight * configuration.Amount);
                    if (exitDetail.PureWeight <= 1000)
                        amount += exitDetail.InputDetail.Product.ProductGroup.InventoryAmount;

                    else
                    {
                        int ton = (int)Math.Ceiling((double)exitDetail.PureWeight / 1000);

                        amount += (exitDetail.InputDetail.Product.ProductGroup.InventoryAmount) * ton;
                    }
                }
                decimal vat = (amount + loadAmount + CutAmount + OtherAmount + WeighbridgeAmount) * (configuration.VAT / 100);
                decimal total = amount + loadAmount + CutAmount + OtherAmount + WeighbridgeAmount + vat;

                ExitAccountViewModel result = new ExitAccountViewModel()
                {
                    InventoryAmount = amount.ToString("N0"),
                    CutAmount = CutAmount.ToString("N0"),
                    LoadAmount = loadAmount.ToString("N0"),
                    OtherAmount = OtherAmount.ToString("N0"),
                    WeighbridgeAmount = WeighbridgeAmount.ToString("N0"),
                    Vat = vat.ToString("N0"),
                    TotalAmount = total.ToString("N0"),

                };

                ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalizeExit(string exitId, string cutAmount, string otherAmount, string weighbridgeAmount,
                                         string loadAmount, string inventoryAmount, string vatAmount,
                                         string receivedInventoryAmount, string receivedCutAmount,
                                         string receivedLoadAmount, string receivedVatAmount,
                                         string customer, string inventoryCustomer, string cutCustomer, string loadCustomer, string vatCustomer)
        {
            try
            {
                Guid id = new Guid(exitId);

                Exit exit = UnitOfWork.ExitRepository.GetById(id);

                ManageConfiguration configuration = UnitOfWork.ManageConfigurationRepository.Get().FirstOrDefault();

                decimal subTotalAmount = Convert.ToDecimal(inventoryAmount) + Convert.ToDecimal(loadAmount) +
                                      Convert.ToDecimal(cutAmount) +
                                      Convert.ToDecimal(otherAmount) + Convert.ToDecimal(weighbridgeAmount);

                decimal vat = subTotalAmount * (configuration.VAT / 100);

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

                decimal totalReceivedAmount = Convert.ToDecimal(receivedInventoryAmount) + Convert.ToDecimal(receivedCutAmount)
                    + Convert.ToDecimal(receivedLoadAmount) + Convert.ToDecimal(receivedVatAmount);

                Payment payment = new Payment()
                {
                    ExitId = exit.Id,
                    InventoryAmount = Convert.ToDecimal(receivedInventoryAmount),
                    CutAmount = Convert.ToDecimal(receivedCutAmount),
                    LoadAmount = Convert.ToDecimal(receivedLoadAmount),
                    VatAmount = Convert.ToDecimal(receivedVatAmount),
                    TotalAmount = totalReceivedAmount,
                    IsDeleted = false,
                    IsActive = true,
                    CreationDate = DateTime.Now
                };
                UnitOfWork.PaymentRepository.Insert(payment);
                UnitOfWork.Save();

                if (Convert.ToDecimal(receivedInventoryAmount) > 0)
                {
                    InsertAccounting(receivedInventoryAmount, inventoryAmount, customer, inventoryCustomer, exit.Code.ToString(), "انبار");
                }
                if (Convert.ToDecimal(receivedCutAmount) > 0)
                {
                    InsertAccounting(receivedCutAmount, cutAmount, customer, cutCustomer, exit.Code.ToString(), "برش");
                }
                if (Convert.ToDecimal(receivedLoadAmount) > 0)
                {
                    InsertAccounting(receivedLoadAmount, loadAmount, customer, loadCustomer, exit.Code.ToString(), "بارگذاری");
                }
                if (Convert.ToDecimal(receivedVatAmount) > 0)
                {
                    InsertAccounting(receivedVatAmount, vatAmount, customer, vatCustomer, exit.Code.ToString(), "ارزش افزوده");
                }
                
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public void InsertAccounting(string receivedAmount, string mainAmount, string customer, string receivedCustomer, string exitCode,string title)
        {
            try
            {
                decimal bedehkar = 0;
                decimal bestankar = 0;
                Guid customerId;
                if (Convert.ToDecimal(receivedAmount) < Convert.ToDecimal(mainAmount))
                {
                    bedehkar = Convert.ToDecimal(mainAmount) - Convert.ToDecimal(receivedAmount);
                    customerId = new Guid(receivedCustomer);
                }
                else
                {
                    bestankar = Convert.ToDecimal(receivedAmount);
                    customerId = new Guid(customer);
                }

               
                Accounting accounting = new Accounting()
                {
                    Code = GetAccountingCode(),
                    Title = "خرج "+ title + " خروجی کد " + exitCode,
                    Bedehkar = bedehkar,
                    Bestankar = bestankar,
                    CustomerId = customerId,
                    IsDeleted = false,
                    IsActive = true,
                    CreationDate = DateTime.Now
                };
                UnitOfWork.AccountingRepository.Insert(accounting);
                UnitOfWork.Save();

                
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public int GetAccountingCode()
        {
            Accounting accounting = UnitOfWork.AccountingRepository.Get().OrderByDescending(x => x.Code).FirstOrDefault();
            if (accounting != null)
            {
                return accounting.Code + 1;
            }
            else
                return 1000;
        }
    }
}
