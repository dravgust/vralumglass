using System.Collections.Generic;
using System.Linq;

namespace Varalumglass.Test
{
    /// <summary>
    /// A generic 'Plank'
    /// </summary>
    public class Plank
    {
        public Plank(float length)
        {
            OriginalLength = length;
        }

        public float FreeLength => OriginalLength - Cuts.Sum();

        public float OriginalLength;

        public readonly List<float> Cuts = new List<float>();

        public void Cut(float cutLength)
        {
            Cuts.Add(cutLength);
        }
    }
}
