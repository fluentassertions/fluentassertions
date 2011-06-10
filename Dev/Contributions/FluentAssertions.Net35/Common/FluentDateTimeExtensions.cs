using System;

namespace FluentAssertions.Common
{
   public static class FluentDateTimeExtensions
   {
      public static DateTime January(this int day, int year)
      {
         return new DateTime(year, 1, day);
      }

      public static DateTime February(this int day, int year)
      {
         return new DateTime(year, 2, day);
      }

      public static DateTime March(this int day, int year)
      {
         return new DateTime(year, 3, day);
      }

      public static DateTime April(this int day, int year)
      {
         return new DateTime(year, 4, day);
      }

      public static DateTime May(this int day, int year)
      {
         return new DateTime(year, 5, day);
      }

      public static DateTime June(this int day, int year)
      {
         return new DateTime(year, 6, day);
      }

      public static DateTime July(this int day, int year)
      {
         return new DateTime(year, 7, day);
      }

      public static DateTime August(this int day, int year)
      {
         return new DateTime(year, 8, day);
      }

      public static DateTime September(this int day, int year)
      {
         return new DateTime(year, 9, day);
      }

      public static DateTime October(this int day, int year)
      {
         return new DateTime(year, 10, day);
      }

      public static DateTime November(this int day, int year)
      {
         return new DateTime(year, 11, day);
      }

      public static DateTime December(this int day, int year)
      {
         return new DateTime(year, 12, day);
      }

      public static DateTime At(this DateTime date, TimeSpan time)
      {
         return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
      }

      public static DateTime At(this DateTime date, int hours, int minutes, int seconds = 0)
      {
         return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
      }
   }
}