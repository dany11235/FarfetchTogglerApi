using AutoMapper;
using FarfetchBusinessEntities;
using FarfetchDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserEntity>();
                cfg.CreateMap<Tokens, TokenEntity>();
                cfg.CreateMap<Services, ServiceEntity>();
                cfg.CreateMap<FeatureToggle, FeatureToggleEntity>();
                cfg.CreateMap<ServiceFeatureToggle, ServiceToggleEntity>();
                
            });
        }
    }
}
