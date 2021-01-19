using System.Globalization;

namespace TaleLearnCode.TwitchCommander.Extensions
{

	public static class IntExtensions
	{

		public static string Display(this int number)
		{
			return number.ToString("N0", CultureInfo.InvariantCulture);
		}

	}

}