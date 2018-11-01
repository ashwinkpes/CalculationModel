using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class AbbRelCareContext : DbContext
    {
        public AbbRelCareContext(DbContextOptions<AbbRelCareContext> options) : base(options)
        {

        }

    

        public DbSet<Asset> Assets { get; set; }
        public DbSet<SubSystem> SubSystems { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
    }
}
