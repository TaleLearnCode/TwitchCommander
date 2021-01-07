using System;

namespace TaleLearnCode.TwitchCommander.Extensions
{

	/// <summary>
	/// Extensions to the <see cref="DateTime"/> type.
	/// </summary>
	public static class DateTimeExtensions
	{

		/// <summary>
		/// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z.
		/// </summary>
		/// <param name="input">The <see cref="DateTime"/> to convert.</param>
		/// <returns>The number of seconds that have elapsed since 1970-01-01T00:00:00Z.</returns>
		public static double ToUnixTimeSeconds(this DateTime input)
		{
			return new DateTimeOffset(input).ToUnixTimeSeconds();
		}

		/// <summary>
		/// Returns the number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.
		/// </summary>
		/// <param name="input">The <see cref="DateTime"/> to convert.</param>
		/// <returns>The number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.</returns>
		public static double ToUnixTimeMilliseconds(this DateTime input)
		{
			return new DateTimeOffset(input).ToUnixTimeMilliseconds();
		}

		/// <summary>
		/// Return the UTC date/time for the specified Unix time (expressed in seconds).
		/// </summary>
		/// <param name="unixTime">The Unix time expressed in seconds.</param>
		/// <returns>A <see cref="DateTime"/> representing the UTC date/time for the specified Unix time.</returns>
		public static DateTime UnixTimeInSecondsToDateTime(double unixTime)
		{
			return DateTimeOffset.FromUnixTimeSeconds((long)unixTime).UtcDateTime;
		}

		/// <summary>
		/// Return the UTC date/time for the specified Unix time (expressed in milliseconds).
		/// </summary>
		/// <param name="unixTime">The Unix time expressed in milliseconds.</param>
		/// <returns>A <see cref="DateTime"/> representing the UTC date/time for the specified Unix time.</returns>
		public static DateTime UnixTimeInMillisecondsToDateTIme(double unixTime)
		{
			return DateTimeOffset.FromUnixTimeMilliseconds((long)unixTime).UtcDateTime;
		}

	}

}