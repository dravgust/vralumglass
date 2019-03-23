using System;
using System.Collections.Generic;
using System.Text;

namespace Vralumglass.Core
{
	public static class FileStorageExtensions
	{
		public static void Using<T>(this T client, Action<T> work) where T : IFileStorage
		{
			//using (client)
			{
				work(client);
			}
		}

		public static TResult Using<T, TResult>(this T client, Func<T, TResult> work) where T : IFileStorage
		{
			//using (client)
			{
				return work(client);
			}
		}
	}
}
