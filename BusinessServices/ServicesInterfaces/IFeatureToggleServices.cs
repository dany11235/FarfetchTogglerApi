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
        bool DeleteFeatureToggle(string featureToggleName);
    }
}
