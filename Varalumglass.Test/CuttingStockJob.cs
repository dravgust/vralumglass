using System;
using System.Collections.Generic;
using System.Linq;

namespace Varalumglass.Test
{
    public class CuttingStockJob
    {
        private readonly List<float> _desiredLengths;

        public CuttingStockJob(List<float> desiredLength)
        {
            _desiredLengths = desiredLength ?? throw new ArgumentNullException(nameof(desiredLength));

            //Perform some basic optimizations
            _desiredLengths.Sort();
            _desiredLengths.Reverse();
        }

        public List<Plank> CalculateCuts(List<float> possibleLengths)
        {
            possibleLengths.Sort();

            var planks = new List<Plank>(); //Buffer list

            //go through cuts
            foreach (var i in _desiredLengths)
            {
                //if no eligible planks can be found
                if (!planks.Any(plank => plank.FreeLength >= i))
                {
                    //make a plank
                    planks.Add(new Plank(possibleLengths.Max()));
                }

                //cut where possible
                foreach (var plank in planks.Where(plank => plank.FreeLength >= i))
                {
                    plank.Cut(i);
                    break;
                }
            }

            //cut down on waste by minimizing length of plank
            foreach (var plank in planks)
            {
                float newLength = plank.OriginalLength;
                foreach (float possibleLength in possibleLengths)
                {
                    if (Math.Abs(possibleLength - plank.OriginalLength) > 0.0 && plank.OriginalLength - plank.FreeLength <= possibleLength)
                    {
                        newLength = possibleLength;
                        break;
                    }
                }
                plank.OriginalLength = newLength;
            }

            return planks;
        }

        //Calculate how much waste/free length is left in a list of planks
        public static float GetFree(IEnumerable<Plank> planks)
        {
            float free = 0;

            foreach (var plank in planks)
            {
                free += plank.FreeLength;
            }
            return free;
        }
    }
}
