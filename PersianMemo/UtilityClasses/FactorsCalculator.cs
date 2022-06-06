using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.UtilityClasses
{
    public class FactorsCalculator
    {
        public double CalculateEF(double oldEF, double q)
        {
            double newEF;
            newEF = oldEF - 0.8 + 0.28 * q - 0.02 * q * q;
            return newEF;
        }

        public double CalculateInterval(double oldInterval, double EF)
        {
            double newInterval;
            newInterval = oldInterval * EF;
            return newInterval;
        }
    }
}
