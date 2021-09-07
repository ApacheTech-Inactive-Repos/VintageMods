using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace VintageMods.Core.Helpers
{
    public static class MEFHelper
    {
        public static void ComposeBatch(this CompositionContainer container, params object[] parts)
        {
            try
            {
                var batch = new CompositionBatch();
                foreach (var o in parts)
                {
                    batch.AddPart(o);
                }
                container.Compose(batch);
            }
            catch (ArgumentNullException)
            {
                
            }
        }
    }
}
