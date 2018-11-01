using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public abstract class Base
    {
       public string Name { get; set; }       

        public DateTime CreatedOn { get; set; }

       public DateTime? UpdatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
