
using System.Data.Entity.ModelConfiguration;
using Resources;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        [Display(Name = "Username", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Username { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }


        [Display(Name = "Password", ResourceType = typeof(Resources.Models.User))]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

      

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

  
        public Guid RoleId { get; set; }

        public string   Email { get; set; }

        public virtual Role Role { get; set; }
        internal class configuration : EntityTypeConfiguration<User>
        {
            public configuration()
            {
                HasRequired(p => p.Parent).WithMany(t => t.Users).HasForeignKey(p => p.ParentId);

                HasRequired(p => p.Role).WithMany(j => j.Users).HasForeignKey(p => p.RoleId);

                
            }
        }
    }
}

