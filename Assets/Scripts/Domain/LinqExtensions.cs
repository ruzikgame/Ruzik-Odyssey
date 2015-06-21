using System.Linq;
using System.Collections.Generic;
using System;

namespace RuzikOdyssey.Domain
{
	public static class LinqExtensions
	{
		public static int XSum(this IEnumerable<GameLevel> collection, Func<GameLevel, int> selector)
		{
			var result = 0;
			foreach (var item in collection) result += selector(item);
			return result;
		}
	}
}
