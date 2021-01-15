using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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