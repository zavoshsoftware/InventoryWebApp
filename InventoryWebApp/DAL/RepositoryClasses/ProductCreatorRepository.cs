﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class ProductCreatorRepository : Repository<ProductCreator>, IProductCreatorRepository
    {
        public ProductCreatorRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
 
    }
}
