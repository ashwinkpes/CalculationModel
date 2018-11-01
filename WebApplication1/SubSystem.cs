using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class SubSystem : Base
    {
        public Guid SubSystemId { get; set; }

        public Guid AssetId { get; set; }

        public int SubSystemWeight { get; set; }

        public double? HealthScore { get; set; }

        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }

        public virtual ICollection<Characteristic> Characteristics { get; set; } = new HashSet<Characteristic>();
    }
}
