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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<FeatureToggle, FeatureToggleEntity>());
                var mapper = config.CreateMapper();
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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<FeatureToggle, FeatureToggleEntity>());
                var mapper = config.CreateMapper();
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
        /// Deletes a particular feature toggle
        /// </summary>
        /// <param name="featureToggleName"></param>
        /// <returns></returns>
        public bool DeleteFeatureToggle(string featureToggleName)
        {
            var success = false;

            using (var scope = new TransactionScope())
            {
                var featureToggle = _unitOfWork.FeatureToggleRepository.GetByID(featureToggleName);
                if (featureToggle != null)
                {
                    _unitOfWork.FeatureToggleRepository.Delete(featureToggle);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }
    }
}
