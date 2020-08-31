using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICityRepository CityRepository { get; }
    
        IProductRepository ProductRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
      
        IProductGroupRepository ProductGroupRepository { get; }
       
        void Save();
    }
}
