﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class ManageConfigurationRepository:Repository<Models.ManageConfiguration>, IManageConfigurationRepository
    {
        public ManageConfigurationRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
    }

}
}
