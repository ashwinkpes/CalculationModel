using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Telemetry 
    {
        public Guid CharacteristicId { get; set; }

        public Guid AssetId { get; set; }

        public DateTime DataRecordedOn { get; set; }

        public double? Value { get; set; }

        public string TextValue { get; set; }
    }
}
