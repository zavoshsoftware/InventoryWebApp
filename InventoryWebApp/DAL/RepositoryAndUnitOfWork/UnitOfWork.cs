using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class UnitOfWork : System.Object, IUnitOfWork
    {
        public UnitOfWork()
        {
            IsDisposed = false;
        }
        protected bool IsDisposed { get; set; }

        private Models.DatabaseContext _databaseContext;
        protected virtual Models.DatabaseContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext =
                        new Models.DatabaseContext();
                }
                return (_databaseContext);
            }
        }
        public void Save()
        {
            try
            {
                DatabaseContext.SaveChanges();
            }
            //catch (System.Exception ex)
            catch
            {
                //Hmx.LogHandler.Report(GetType(), null, ex);
                throw;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _databaseContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Inserting custom Respositories
        //private IRepository _Repository;
        //public IRepository Repository
        //{
        //	get
        //	{
        //		if (_Repository == null)
        //		{
        //			_Repository =
        //				new Repository(DatabaseContext);
        //		}
        //		return (_Repository);
        //	}
        //}

        

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository =
                        new UserRepository(DatabaseContext);
                }
                return (_userRepository);
            }
        }

      
 

        private IRoleRepository _roleRepository;
        public IRoleRepository  RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository =
                        new RoleRepository(DatabaseContext);
                }
                return (_roleRepository);
            }
        }


        private ICityRepository _cityRepository;
        public ICityRepository CityRepository
        {
            get
            {
                if (_cityRepository == null)
                {
                    _cityRepository =
                        new CityRepository(DatabaseContext);
                }
                return (_cityRepository);
            }
        }

  

        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository =
                        new ProductRepository(DatabaseContext);
                }
                return (_productRepository);
            }
        }

     
        private IProvinceRepository _provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (_provinceRepository == null)
                {
                    _provinceRepository =
                        new ProvinceRepository(DatabaseContext);
                }
                return (_provinceRepository);
            }
        }
  

        private IProductGroupRepository _productGroupRepository;
        public IProductGroupRepository ProductGroupRepository
        {
            get
            {
                if (_productGroupRepository == null)
                {
                    _productGroupRepository =
                        new ProductGroupRepository(DatabaseContext);
                }
                return (_productGroupRepository);
            }
        }


        private ITransporterRepository _transporterRepository;
        public ITransporterRepository TransporterRepository
        {
            get
            {
                if (_transporterRepository == null)
                {
                    _transporterRepository =
                        new TransporterRepository(DatabaseContext);
                }
                return (_transporterRepository);
            }
        }


        private ICustomerRepository _customerRepository;
        public ICustomerRepository  CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository =
                        new CustomerRepository(DatabaseContext);
                }
                return (_customerRepository);
            }
        }


        private IProductGroupUnitRepository _productGroupUnitRepository;
        public IProductGroupUnitRepository ProductGroupUnitRepository
        {
            get
            {
                if (_productGroupUnitRepository == null)
                {
                    _productGroupUnitRepository =
                        new ProductGroupUnitRepository(DatabaseContext);
                }
                return (_productGroupUnitRepository);
            }
        }


        private IInputRepository _inputRepository;
        public IInputRepository InputRepository
        {
            get
            {
                if (_inputRepository == null)
                {
                    _inputRepository =
                        new InputRepository(DatabaseContext);
                }
                return (_inputRepository);
            }
        }

        private IInputDetailsRepository _inputDetailsRepository;
        public IInputDetailsRepository InputDetailsRepository
        {
            get
            {
                if (_inputDetailsRepository == null)
                {
                    _inputDetailsRepository =
                        new InputDetailsRepository(DatabaseContext);
                }
                return (_inputDetailsRepository);
            }
        }

        private IProductCreatorRepository _productCreatorRepository;
        public IProductCreatorRepository  ProductCreatorRepository
        {
            get
            {
                if (_productCreatorRepository == null)
                {
                    _productCreatorRepository =
                        new ProductCreatorRepository(DatabaseContext);
                }
                return (_productCreatorRepository);
            }
        }

        private IProductFormRepository _productFormRepository;
        public IProductFormRepository ProductFormRepository
        {
            get
            {
                if (_productFormRepository == null)
                {
                    _productFormRepository =
                        new ProductFormRepository(DatabaseContext);
                }
                return (_productFormRepository);
            }
        }


        private IProductStatusRepository _productStatusRepository;
        public IProductStatusRepository ProductStatusRepository
        {
            get
            {
                if (_productStatusRepository == null)
                {
                    _productStatusRepository =
                        new ProductStatusRepository(DatabaseContext);
                }
                return (_productStatusRepository);
            }
        }

        #endregion Inserting custom Respositories

    }
}
