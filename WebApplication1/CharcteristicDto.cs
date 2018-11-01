using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class CharcteristicDto
    {
        public Guid CharacteristicId { get; set; }
        public int Weight { get; set; }
        public int NewWeight { get; set; }
        public double HealthScore { get; set; }
        public int OrderNo { get; set; }
        public double WeightedHealthScore { get; set; }      

    }
}
