using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class InputDetailStatusRepository : Repository<Models.InputDetailStatus>, IInputDetailStatusRepository
    {
        public InputDetailStatusRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
