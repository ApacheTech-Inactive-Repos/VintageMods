using System;

namespace VintageMods.Core.Legacy.Maths.Vectors
{
    public abstract class Vector<T> where T: struct
    {
        public int Dimensions { get; }

        protected readonly T[] Values;

        protected Vector(int dimensions)
        {
            Dimensions = dimensions;
            Values = new T[dimensions];
        }

        public T GetValueByDimension(int dimension)
        {
            if (dimension >= 0 && dimension < Dimensions) return Values[dimension];
            throw new IndexOutOfRangeException("Index outside the bounds of the vector.");
        }

        public void SetValueByDimension(int dimension, T value)
        {
            if (dimension >= 0 && dimension < Dimensions)
                Values[dimension] = value;
            else
                throw new IndexOutOfRangeException("Index outside the bounds of the vector.");
        }
    }
}