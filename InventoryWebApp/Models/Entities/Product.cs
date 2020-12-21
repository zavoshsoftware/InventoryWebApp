using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    
    public class Product : BaseEntity
    {
        public Product()
        {
            InputDetails=new List<InputDetail>();
            Orders=new List<Order>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        public string Title { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroup))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductGroupId { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductForm))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductFormId { get; set; }

        public virtual ProductForm ProductForm { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductCreator))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductCreatorId { get; set; }

        public virtual ProductCreator ProductCreator { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductStatus))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductStatusId { get; set; }

        public virtual ProductStatus ProductStatus { get; set; }

        [Display(Name = "Length", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Length { get; set; }

        [Display(Name = "Thickness", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Thickness { get; set; }

        [Display(Name = "Width", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Width { get; set; }

        [Display(Name = "Weight", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal Weight { get; set; }

        [Display(Name = "IsPureWeight", ResourceType = typeof(Resources.Models.Product))]
        public bool IsPureWeight { get; set; }

        [Display(Name = "Grid", ResourceType = typeof(Resources.Models.Product))]
        public string Grid { get; set; }

        [Display(Name = "Other", ResourceType = typeof(Resources.Models.Product))]
        public string Other { get; set; }

        public virtual ICollection<InputDetail> InputDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
