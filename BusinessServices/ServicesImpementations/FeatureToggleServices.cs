using System;
using System.Collections.Generic;
using FarfetchBusinessEntities;
using FarfetchDataModel.UnitOfWork;
using FarfetchBusinessServices.ServicesInterfaces;
using AutoMapper;
using FarfetchDataModel;
using System.Linq;
using System.Transactions;

namespace FarfetchBusinessServices.ServicesImpementations
{
    public class FeatureToggleServices : IFeatureToggleServices
    {

        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public FeatureToggleServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        /// <summary>
        /// Fetches feature toggle details by name/id
        /// </summary>
        /// <param name="featureToggleName"></param>
        /// <returns></returns>
        public FeatureToggleEntity GetFeatureToggleByName(string featureToggleName)
        {
            var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(featureToggleName);
            if (featureToggle != null)
            {
                
                var featureToggleModel = Mapper.Map<FeatureToggle, FeatureToggleEntity>(featureToggle);
                return featureToggleModel;
            }
            return null;
        }


        /// <summary>
        /// Fetches all the feature toggles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FeatureToggleEntity> GetAllFeatureToggles()
        {
            var featureToggles = _unitOfWork.FeatureToggleRepository.GetAll().ToList();
            if (featureToggles.Any())
            {            
                var featureTogglesModel = Mapper.Map<List<FeatureToggle>, List<FeatureToggleEntity>>(featureToggles);
                return featureTogglesModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a feature toggle
        /// </summary>
        /// <param name="serviceEntity"></param>
        /// <returns></returns>
        public string CreateFeatureToggle(FeatureToggleEntity featureToggleEntity)
        {
            using (var scope = new TransactionScope())
            {
                var featureToggle = new FeatureToggle
                {
                    Name = featureToggleEntity.Name,
                    Value = featureToggleEntity.Value
                };
                _unitOfWork.FeatureToggleRepository.Insert(featureToggle);
                _unitOfWork.Save();
                scope.Complete();
                return featureToggle.Name;
            }
        }


        /// <summary>
        /// Updates a feature toggle value
        /// </summary>
        /// <param name="featureToggleName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateFeatureToggle(string featureToggleName, bool value)
        {
            var success = false;
           
            using (var scope = new TransactionScope())
            {
                var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(featureToggleName);
                if (featureToggle != null)
                {
                    featureToggle.Value = value;
                    _unitOfWork.FeatureToggleRepository.Update(featureToggle);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }
        
            return success;
        }

       
        /// <summary>
        /// Gets services configurations for a feature toggle
        /// </summary>
        /// <param name="featureToggleName"></param>
        /// <returns></returns>

        public IEnumerable<ServiceToggleEntity> GetFeatureToggleServicesConfigurations(string featureToggleName) {

            var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(featureToggleName);
            if (featureToggle != null)
            {
                var servicesCofiguration = _unitOfWork.ServiceFeatureToggleRepository.GetManyQueryable(p => p.FeatureToggleName == featureToggleName);
              
                var servicesConfigurationModel = Mapper.Map<List<ServiceFeatureToggle>, List<ServiceToggleEntity>>(servicesCofiguration.ToList());
                return servicesConfigurationModel;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets a feature toggle service configuration
        /// </summary>
        /// <param name="featureToggleName"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>

        public ServiceToggleEntity GetFeatureToggleServiceConfiguration(string featureToggleName, int serviceId) {
            var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(featureToggleName);
            if (featureToggle != null)
            {
                var serviceCofiguration = _unitOfWork.ServiceFeatureToggleRepository.GetByID(serviceId,featureToggleName);

                var serviceConfigurationModel = Mapper.Map<ServiceFeatureToggle, ServiceToggleEntity>(serviceCofiguration);
                return serviceConfigurationModel;
            }
            else
                return null;
        }

        public ServiceToggleEntity InsertFeatureToggleServiceConfiguration(string featureToggleName, ServiceToggleEntity serviceToggleEntity) {

            using (var scope = new TransactionScope())
            {
                var servicefeatureToggle = new ServiceFeatureToggle
                {
                    FeatureToggleName = featureToggleName,
                    ServiceId = serviceToggleEntity.ServiceId,
                    CustomValue = serviceToggleEntity.CustomValue,

                };
                _unitOfWork.ServiceFeatureToggleRepository.Insert(servicefeatureToggle);
                _unitOfWork.Save();
                scope.Complete();
                return serviceToggleEntity;
            }
        }

        public bool UpdateFeatureToggleServiceConfiguration(string featureToggleName, int serviceId, ServiceToggleEntity serviceToggleEntity) {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var serviceFeatureToggle = _unitOfWork.ServiceFeatureToggleRepository.GetByID(serviceId,featureToggleName);
                if (serviceFeatureToggle != null)
                {
                    serviceFeatureToggle.CustomValue = serviceToggleEntity.CustomValue;
                    _unitOfWork.ServiceFeatureToggleRepository.Update(serviceFeatureToggle);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }



        public bool DeleteFeatureToggleServiceConfiguration(int serviceId,string featureToggleName)
        {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var serviceFeatureToggle = _unitOfWork.ServiceFeatureToggleRepository.GetByID(serviceId,featureToggleName);
                if (serviceFeatureToggle != null)
                {
                    
                    _unitOfWork.ServiceFeatureToggleRepository.Delete(serviceFeatureToggle);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }


        public IEnumerable<FeatureToggleEntity> GetServiceAvailableToggles(int serviceId, string serviceVersion) {

            var service = _unitOfWork.ServiceRepository.GetSingle(p=>p.ServiceId == serviceId && p.ServiceVersion == serviceVersion);

            if (service != null)
            {
                var availableToggles = new List<FeatureToggleEntity>();
                var toggleConfigurations= _unitOfWork.ServiceFeatureToggleRepository.GetMany(p=>p.ServiceId == serviceId && p.CustomValue != false);

                foreach (var toggleConfiguration in toggleConfigurations) {

                    if (toggleConfiguration.CustomValue == true)
                    {
                        availableToggles.Add(new FeatureToggleEntity
                        {
                            Name = toggleConfiguration.FeatureToggleName,
                            Value = true
                        });
                    }
                    else {
                        var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(toggleConfiguration.FeatureToggleName);
                        if (featureToggle.Value) {
                            availableToggles.Add(new FeatureToggleEntity
                            {
                                Name = toggleConfiguration.FeatureToggleName,
                                Value = true
                            });
                        }
                    }

                }

                return availableToggles;

            }
            else {
                return null;
            }

        }

    }
}
