using FarfetchBusinessServices;
using FarfetchBusinessServices.Interfaces;
using FarfetchBusinessServices.ServicesImpementations;
using FarfetchBusinessServices.ServicesInterfaces;
using FarfetchDependencyResolver;
using System.ComponentModel.Composition;


namespace BusinessServices
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IServiceApplicationServices, ServiceApplicationServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<ITokenServices, TokenServices>();
            registerComponent.RegisterType<IFeatureToggleServices, FeatureToggleServices>();
            
        }
    }
}
