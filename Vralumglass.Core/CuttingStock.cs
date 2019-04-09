using System;
using System.Collections.Generic;
using System.Linq;
using Vralumglass.Core.Interfaces;
using Vralumglass.Core.Models;

namespace Vralumglass.Core
{
    public class CuttingStock
    {
        private readonly List<ISnippet> _snippets;

        private float MaxSnippetLength => _snippets.Max(l => l.Length);

        public CuttingStock(List<ISnippet> snippets)
        {
            _snippets = snippets ?? throw new ArgumentNullException(nameof(snippets));

            _snippets.Sort();
            _snippets.Reverse();
        }

        public List<Plank> OptimizedCuts(int numberOfOptions = 1)
        {
            var possibleLengths = new List<float> { MaxSnippetLength };

            return CalculateCuts(possibleLengths);
        }

        public List<Plank> CalculateCuts(List<float> possibleLengths)
        {
            if (possibleLengths == null)
            {
                possibleLengths = new List<float> { MaxSnippetLength };
            }
            else if (!possibleLengths.Any() || possibleLengths.Max() < MaxSnippetLength)
            {
                possibleLengths.Add(MaxSnippetLength);
            }

            possibleLengths.Sort();

            return Calculate(possibleLengths);
        }

        private List<Plank> Calculate(IReadOnlyCollection<float> possibleLengths)
        {
            var planks = new List<Plank>();

            foreach (var i in _snippets)
            {
                //if no eligible planks can be found
                if (!planks.Any(plank => plank.FreeLength >= i.Length))
                {
                    //make a plank
                    planks.Add(new Plank(possibleLengths.Max()));
                }

                //cut where possible
                foreach (var plank in planks.Where(plank => plank.FreeLength >= i.Length))
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
