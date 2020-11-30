using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class ProductGroupCustomActionRepository : Repository<Models.ProductGroupCustomAction>, IProductGroupCustomActionRepository
    {
        public ProductGroupCustomActionRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
