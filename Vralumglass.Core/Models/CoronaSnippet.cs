using Vralumglass.Core.Interfaces;

namespace Vralumglass.Core.Models
{
    public class CoronaSnippet : Snippet
    {
        public CoronaSnippet(float length) : base(length)
        {

        }

        public string Apartment { get; set; }

        public string Floor { get; set; }

        public string Columns { get; set; }

        public override string ToString()
        {
            return $"{Length}[{Floor}/{Apartment}]";
        }

        public override ISnippet Clone(float length)
        {
            return new CoronaSnippet(length)
            {
                Apartment = Apartment,
                Floor = Floor,
                Columns = Columns
            };
        }
    }
}
