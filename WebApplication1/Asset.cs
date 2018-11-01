using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Asset : Base
    {
        [Key]
        public Guid AssetId { get; set; }

        public int? HealthScoreQuality { get; set; }

        public double? HealthScore { get; set; }

        public virtual ICollection<SubSystem> SubSystems { get; set; } = new HashSet<SubSystem>();
    }
}
