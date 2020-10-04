using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class CutDetailTypeRepository:Repository<Models.CutDetailType>, ICutDetailTypeRepository
    {
        public CutDetailTypeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
