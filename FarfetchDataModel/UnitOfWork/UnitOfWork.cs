using FarfetchDataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarfetchDataModel.UnitOfWork
{
    public class UnitOfWork : IDisposable,IUnitOfWork
    {
        #region Private member variables...

        private WebApiDbEntities _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Services> _serviceRepository;
        private GenericRepository<Tokens> _tokenRepository;
        private GenericRepository<FeatureToggle> _featureToggleRepository;
        private GenericRepository<ServiceFeatureToggle> _serviceFeatureToggleRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new WebApiDbEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for Service repository.
        /// </summary>
        public GenericRepository<Services> ServiceRepository
        {
            get
            {
                if (this._serviceRepository == null)
                    this._serviceRepository = new GenericRepository<Services>(_context);
                return _serviceRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Tokens> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                    this._tokenRepository = new GenericRepository<Tokens>(_context);
                return _tokenRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<FeatureToggle> FeatureToggleRepository
        {
            get
            {
                if (this._featureToggleRepository == null)
                    this._featureToggleRepository = new GenericRepository<FeatureToggle>(_context);
                return _featureToggleRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for ServiceFeatureToggle repository.
        /// </summary>
        public GenericRepository<ServiceFeatureToggle> ServiceFeatureToggleRepository
        {
            get
            {
                if (this._serviceFeatureToggleRepository == null)
                    this._serviceFeatureToggleRepository = new GenericRepository<ServiceFeatureToggle>(_context);
                return _serviceFeatureToggleRepository;
            }
        }
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, 
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false; 
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}
