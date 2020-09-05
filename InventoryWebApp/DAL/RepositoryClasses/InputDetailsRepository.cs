using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class  InputDetailsRepository : Repository<Models.InputDetail>, IInputDetailsRepository
    {
        public InputDetailsRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
