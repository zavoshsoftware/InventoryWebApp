using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class TempController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public string ConvertProducts()
        {
          DatabaseContext db=new DatabaseContext();
            var products = db.TempProducts;

            foreach (var product in products)
            {
                var productGroup = UnitOfWork.ProductGroupRepository.Get(c => c.Code == product.Product_Type_Code).FirstOrDefault();

                if (productGroup != null)
                {
                    ProductCreator productCreator = UnitOfWork.ProductCreatorRepository
                        .Get(c => c.Title == product.Maker).FirstOrDefault();

                    Guid? creatorId = null;
                    if (productCreator != null)
                        creatorId = productCreator.Id;

                    ProductForm productForm = UnitOfWork.ProductFormRepository.Get(c => c.Title == product.Type)
                        .FirstOrDefault();

                    Guid? productFormId = null;
                    if (productForm != null)
                        productFormId = productForm.Id;

                    Product oProduct = new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = product.Product_Name,
                        ProductGroupId = productGroup.Id,
                        Length = product.Length,
                        Weight = product.App_Weight,
                        IsPureWeight = product.Net_weight,
                        Grade = product.Grade,
                        Width = product.Height,
                        Thickness = product.Width,
                        ProductCreatorId= creatorId,
                        ProductFormId = productFormId,
                        Other = product.Other,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now

                    };
                    UnitOfWork.ProductRepository.Insert(oProduct);
                    //db.Products.Add(oProduct);
                    //db.SaveChanges();
                    UnitOfWork.Save();
                }
            }
            UnitOfWork.Save();

            return "";
        }

        //public ActionResult ConvertMaker()
        //{

        //}
    }
}