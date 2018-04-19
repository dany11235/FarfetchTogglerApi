using AutoMapper;
using FarfetchBusinessEntities;
using FarfetchDataModel;
using FarfetchDataModel.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;


namespace FarfetchBusinessServices.ServicesImpementations
{
   
        /// <summary>
        /// Offers services for service/application specific CRUD operations
        /// </summary>
        public class ServiceApplicationServices : IServiceApplicationServices
        {
            private readonly UnitOfWork _unitOfWork;

            /// <summary>
            /// Public constructor.
            /// </summary>
            public ServiceApplicationServices(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            /// <summary>
            /// Fetches service details by id
            /// </summary>
            /// <param name="serviceId"></param>
            /// <returns></returns>
            public ServiceEntity GetServiceById(int serviceId)
            {
                var service = _unitOfWork.ServiceRepository.GetByID(serviceId);
                if (service != null)
                {
                    
                    var servicesModel = Mapper.Map<Services, ServiceEntity>(service);
                    return servicesModel;
                }
                return null;
            }

            /// <summary>
            /// Fetches all the services.
            /// </summary>
            /// <returns></returns>
            public IEnumerable<ServiceEntity> GetAllServices()
            {
                var services = _unitOfWork.ServiceRepository.GetAll().ToList();
                if (services.Any())
                {
                    // Mapper.CreateMap<Product, ProductEntity>();
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Services, ServiceEntity>());
                    var mapper = config.CreateMapper();
                    var servicesModel = Mapper.Map<List<Services>, List<ServiceEntity>>(services);
                    return servicesModel;
                }
                return null;
            }

            /// <summary>
            /// Creates a service
            /// </summary>
            /// <param name="serviceEntity"></param>
            /// <returns></returns>
            public int CreateService(ServiceEntity serviceEntity)
            {
                using (var scope = new TransactionScope())
                {
                    var service = new Services
                    {
                        ServiceName = serviceEntity.ServiceName,
                        ServiceVersion = serviceEntity.ServiceVersion
                    };
                    _unitOfWork.ServiceRepository.Insert(service);
                    _unitOfWork.Save();
                    scope.Complete();
                    return service.ServiceId;
                }
            }

            /// <summary>
            /// Updates a service
            /// </summary>
            /// <param name="serviceId"></param>
            /// <param name="serviceEntity"></param>
            /// <returns></returns>
            public bool UpdateService(int serviceId,ServiceEntity serviceEntity)
            {
                var success = false;
                if (serviceEntity != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        var service = _unitOfWork.ServiceRepository.GetByID(serviceId);
                        if (service != null)
                        {
                            service.ServiceName = serviceEntity.ServiceName;
                            _unitOfWork.ServiceRepository.Update(service);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
                return success;
            }

        

       
    }
}

