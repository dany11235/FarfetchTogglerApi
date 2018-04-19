using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarfetchBusinessEntities
{
    public class FeatureToggleEntity
    {
        public string Name { get; set; }
        public bool Value { get; set; }
        public ICollection<ServiceToggleEntity> ServiceFeatureToggle { get; set; }
    }
}
