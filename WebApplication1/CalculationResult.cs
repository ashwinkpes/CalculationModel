using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApplication1.Utility;

namespace WebApplication1
{
    public class CalculationResult
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Value { get; set; }
        public string TextValue { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
        public double Value4 { get; set; }
        public double RelativeValue { get; set; }       
    }


    public class CharcteristicHealthData
    {
        public Guid CharacteristicId { get; set; }

        public int? CharcacteristicHs { get; set; }

        public int? CharcacteristicHsq { get; set; }

        public Guid SubsystemId { get; set; }   
        
    }
}
