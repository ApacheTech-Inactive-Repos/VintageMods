using VintageMods.Core.Legacy.Maths.Vectors;

namespace VintageMods.Core.Legacy.Maths.Extensions
{
    public static class VectorEx
    {
        public static T Scale<T>(this T vec, double scaleFactor) where T : Vector<double>, new()
        {
            var retVec = new T();
            for (var i = 0; i < vec.Dimensions; i++)
            {
                var val = vec.GetValueByDimension(i) * scaleFactor;
                retVec.SetValueByDimension(i, val);
            }

            return retVec;
        }
    }
}
