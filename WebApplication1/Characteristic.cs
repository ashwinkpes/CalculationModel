using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static WebApplication1.Utility;

namespace WebApplication1
{
    public class Characteristic : Base
    {
      
        [Key]
        public Guid CharacteristicId { get; set; }

        public int UsedInCalculation { get; set; }

        public int Weight { get; set; }

        public CalculationType CalculationType { get; set; }

        public double RelativeBaseValue { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public double Value1 { get; set; }

        public double Value2 { get; set; }

        public double Value3 { get; set; }

        public double Value4 { get; set; }

        public int DataAgeScale { get; set; }

        public int DataAgeMax { get; set; }

        public string Unit { get; set; }

        public DateTime? TimeStamp { get; set; }

        public double? Value { get; set; }

        public int? HealthScore { get; set; }

        public int? HealthScoreQuality { get; set; }

        public bool IsMandatory { get; set; }

        public Guid SubSystemId { get; set; }

        [ForeignKey("SubSystemId")]
        public virtual SubSystem SubSystem { get; set; }

    }
}
