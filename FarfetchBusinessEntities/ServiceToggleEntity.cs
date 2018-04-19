using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarfetchBusinessEntities
{
    public class ServiceToggleEntity
    {
        public int ServiceId { get; set; }
        public string FeatureToggleName { get; set; }
        public bool CustomValue { get; set; }
    }
}
