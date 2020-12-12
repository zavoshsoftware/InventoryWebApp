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
        ITransporterRepository TransporterRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IProductGroupUnitRepository ProductGroupUnitRepository { get; }
        IInputRepository InputRepository { get; }
        IInputDetailsRepository InputDetailsRepository { get; }
        IProductCreatorRepository ProductCreatorRepository { get; }
        IProductFormRepository ProductFormRepository { get; }
        IProductStatusRepository ProductStatusRepository { get; }
        IOrderRepository OrderRepository { get; }
        IExitRepository ExitRepository { get; }
        IExitDetailRepository ExitDetailRepository { get; }
        IInputDetailStatusRepository InputDetailStatusRepository { get; }
        ICutTypeRepository CutTypeRepository { get; }
        ICutOrderRepository CutOrderRepository { get; }
        ICutDetailTypeRepository CutDetailTypeRepository { get; }
        ICutOrderDetailRepository CutOrderDetailRepository { get; }
        IExitDriverRepository ExitDriverRepository { get; }
        ICustomActionRepository CustomActionRepository { get; }
        IProductGroupCustomActionRepository ProductGroupCustomActionRepository { get; }
        IManageConfigurationRepository ManageConfigurationRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IAccountingRepository AccountingRepository { get; }


        void Save();
    }
}
