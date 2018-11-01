using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class DataSeed
    {
        public static void EnsureSeedData(this AbbRelCareContext db, SeedData seedData)
        {
            if (!db.Assets.Any())
            {
                seedData.Assets.ToList().ForEach(x => { x.CreatedOn = DateTime.Now; x.IsActive = true; });
                db.Assets.AddRange(seedData.Assets);
            }

            if (!db.SubSystems.Any())
            {
                seedData.SubSystems.ToList().ForEach(x => { x.CreatedOn = DateTime.Now; x.IsActive = true; });
                db.SubSystems.AddRange(seedData.SubSystems);
            }

            if (!db.Characteristics.Any())
            {
                seedData.Characteristics.ToList().ForEach(x => { x.CreatedOn = DateTime.Now; x.IsActive = true; });
                db.Characteristics.AddRange(seedData.Characteristics);
            }

            db.SaveChanges();
        }
    }
}
