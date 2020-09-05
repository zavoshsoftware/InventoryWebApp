using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductGroupUnitRepository : Repository<Models.ProductGroupUnit>, IProductGroupUnitRepository
    {
        public ProductGroupUnitRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
