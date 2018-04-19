using FarfetchBusinessEntities;
using System.Collections.Generic;

namespace FarfetchBusinessServices
{
    public interface IServiceApplicationServices
    {
        ServiceEntity GetServiceById(int serviceId);
        IEnumerable<ServiceEntity> GetAllServices();
        int CreateService(ServiceEntity serviceEntity);
        bool UpdateService(int serviceId, ServiceEntity serviceEntity);
    }
}
