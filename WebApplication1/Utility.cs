using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class Utility
    {
        public enum CalculationType
        {
            Type1 = 1,
            Type2 = 2,
            Type3 = 3,
            Type4 = 4,
            Type5= 5,
            Type6 = 6,
            Type7 = 7,
            Type8 = 8,
            Type9 = 9,
            Type10 = 10,
            Type11 = 11,
            Type12 = 12,
            Type13 = 13,
        }

        public enum Status
        {
            CalculationNotPerformed = 1,
            CharcteristicHsCalculated = 1,
            CharcteristicHsqCalculated = 2,
            SubsystemHsCalculated = 3,
            AssetHsCalculated = 4         
        }
    }
}
