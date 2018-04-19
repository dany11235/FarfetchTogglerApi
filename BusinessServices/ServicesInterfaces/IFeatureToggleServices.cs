using FarfetchBusinessEntities;
using System.Collections.Generic;


namespace FarfetchBusinessServices.ServicesInterfaces
{
    public interface IFeatureToggleServices
    {
        FeatureToggleEntity GetFeatureToggleByName(string featureToggleName);
        IEnumerable<FeatureToggleEntity> GetAllFeatureToggles();
        string CreateFeatureToggle(FeatureToggleEntity featureToggleEntity);
        bool UpdateFeatureToggle(string featureToggleName, bool value);
        IEnumerable<ServiceToggleEntity> GetFeatureToggleServicesConfigurations(string featureToggleName);

        ServiceToggleEntity GetFeatureToggleServiceConfiguration(string featureToggleName, int serviceId);

        ServiceToggleEntity InsertFeatureToggleServiceConfiguration(string featureToggleName, ServiceToggleEntity serviceToggleEntity);

        bool UpdateFeatureToggleServiceConfiguration(string featureToggleName, int serviceId, ServiceToggleEntity serviceToggleEntity);

        bool DeleteFeatureToggleServiceConfiguration(int serviceId,string featureToggleName);

        IEnumerable<FeatureToggleEntity> GetServiceAvailableToggles(int serviceId, string serviceVersion);
    }
}
