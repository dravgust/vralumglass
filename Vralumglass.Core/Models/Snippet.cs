using Vralumglass.Core.Interfaces;

namespace Vralumglass.Core.Models
{
    public class Snippet : ISnippet
    {
        public float Length { get; }

        public Snippet(float length)
        {
            Length = length;
        }

        public int CompareTo(ISnippet other)
        {
            return this.Length.CompareTo(other.Length);
        }

        public override string ToString()
        {
            return $"{Length}";
        }

        public virtual ISnippet Clone(float length)
        {
            return new Snippet(length);
        }
    }
}
