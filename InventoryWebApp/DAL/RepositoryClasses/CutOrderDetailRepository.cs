using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class CutOrderDetailRepository: Repository<Models.CutOrderDetail>, ICutOrderDetailRepository
    {
        public CutOrderDetailRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

    }
}
}
