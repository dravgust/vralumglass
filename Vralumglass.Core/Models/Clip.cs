namespace Vralumglass.Core.Models
{
    public class Clip
    {
        public Clip(int id, double weight)
        {
            Id = id;
            Weight = weight;
        }

        public int Id { get; }

        public string Name => $"{Id}";

        public double Weight { get; }
    }
}
