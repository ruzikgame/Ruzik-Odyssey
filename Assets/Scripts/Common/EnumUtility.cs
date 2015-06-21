using System;

namespace RuzikOdyssey.Common
{
	public static class EnumUtility
	{
		public static string GetName<TEnum>(TEnum value)
		{
			return Enum.GetName(typeof(TEnum), value);
		}
	}
}
