using System;

namespace Vralumglass.Core.Interfaces
{
	public interface ISnippet : IComparable<ISnippet>
	{
		float Length { get; }
	}

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
	}
}
