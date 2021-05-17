using System;
using System.Collections.Generic;

namespace VintageMods.Core.Maths.Interpolation
{
    public abstract class InterpolationBase : IInterpolator
    {
        protected readonly Dictionary<double, double> Points = new Dictionary<double, double>();
        protected readonly List<double> PointVecs;

        public InterpolationBase(double[] times, double[] points)
        {
            if (points.Length < 2)
                throw new ArgumentException("At least two points are needed!", nameof(points));

            if (times.Length != points.Length)
                throw new ArgumentException("Invalid times array!", nameof(times));

            for (var i = 0; i < points.Length; i++) Points.Add(times[i], points[i]);
            PointVecs = new List<double>(points);
        }


        public InterpolationBase(params double[] points)
        {
            if (points.Length < 2)
                throw new ArgumentException("At least two points are needed!", nameof(points));

            double time = 0;
            var stepLength = 1D / (points.Length - 1);
            foreach (var t in points)
            {
                Points.Add(time, t);
                time += stepLength;
            }

            PointVecs = new List<double>(points);
        }

        protected virtual double GetValue(int index)
        {
            return PointVecs[index];
        }

        public double ValueAt(double t)
        {
            if (!(t >= 0) || !(t <= 1)) return default;
            KeyValuePair<double, double> firstPoint;
            var indexFirst = -1;

            KeyValuePair<double, double> secondPoint;
            var indexSecond = -1;

            var i = 0;
            foreach (var entry in Points)
            {
                if (entry.Key >= t)
                {
                    if (firstPoint.Equals(default(KeyValuePair<double, double>)))
                    {
                        firstPoint = entry;
                        indexFirst = i;
                    }
                    else
                    {
                        secondPoint = entry;
                        indexSecond = i;
                    }
                    break;
                }
                firstPoint = entry;
                indexFirst = i;
                i++;
            }

            if (secondPoint.Equals(default(KeyValuePair<double, double>)))
                return firstPoint.Value;

            var pointDistance = secondPoint.Key - firstPoint.Key;
            var mu = (t - firstPoint.Key) / pointDistance;
            return ValueAt(mu, indexFirst, indexSecond);

        }

        public abstract double ValueAt(double mu, int pointIndex, int pointIndexNext);
    }
}