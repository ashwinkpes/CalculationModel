using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class SeedData
    {
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<SubSystem> SubSystems { get; set; }
        public IEnumerable<Characteristic> Characteristics { get; set; }
    }
}
