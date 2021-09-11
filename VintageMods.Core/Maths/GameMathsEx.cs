using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.MathTools;

namespace VintageMods.Core.Maths
{
    public class GameMathsEx
    {
        public static double DistributiveProduct(double a1, double a2, double b1, double b2)
        {
            var first = a1 * b1;
            var outside = a1 * b2;
            var inside = a2 * b1;
            var last = a2 * b2;
            return first + outside + inside + last;
        }

        public static float DistributiveProduct(float a1, float a2, float b1, float b2)
        {
            var first = a1 * b1;
            var outside = a1 * b2;
            var inside = a2 * b1;
            var last = a2 * b2;
            return first + outside + inside + last;
        }
    }
}
