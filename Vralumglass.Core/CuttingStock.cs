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

        public CuttingStock(List<ISnippet> snippets)
        {
            _snippets = snippets ?? throw new ArgumentNullException(nameof(snippets)); 
        }

        public List<Plank> CalculateCuts(List<float> plankLengths, float minSnippetLength = 1000)
        {
            if (plankLengths == null || !plankLengths.Any())
                throw new ArgumentException(nameof(plankLengths));

            plankLengths.Sort();

            var maxPlankLength = plankLengths.Last();

            var snippets = new List<ISnippet>();
            var overLengthSnippets = new List<ISnippet>();

            foreach (var snippet in _snippets)
            {
                if (snippet.Length > maxPlankLength)
                {
                    overLengthSnippets.Add(snippet);
                }
                else
                {
                    snippets.Add(snippet);
                }
            }

            foreach (var snippet in overLengthSnippets)
            {
                var snippetLength = snippet.Length;
                do
                {
                    var possibleLength = snippetLength - maxPlankLength;
                    if (possibleLength < minSnippetLength)
                    {
                        maxPlankLength -= minSnippetLength; continue;
                    }

                    snippets.Add(snippet.Clone(maxPlankLength));

                    snippetLength = possibleLength;
                } while (snippetLength > maxPlankLength);

                snippets.Add(snippet.Clone(snippetLength));
            }

            snippets.Sort();
            snippets.Reverse();

            return Calculate(plankLengths, snippets);
        }

        private static List<Plank> Calculate(IReadOnlyCollection<float> plankLengths, IEnumerable<ISnippet> snippets)
        {
            var planks = new List<Plank>();

            foreach (var i in snippets)
            {
                //if no eligible planks can be found
                if (!planks.Any(plank => plank.FreeLength >= i.Length))
                {
                    //make a plank
                    planks.Add(new Plank(plankLengths.Max()));
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
                foreach (float possibleLength in plankLengths)
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
