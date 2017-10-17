using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Utilities
{
  public static class ComputerTime
  {
    [DllImport("coredll.dll")]
    private extern static void GetSystemTime(ref SystemTime lpSystemTime);

    [DllImport("coredll.dll")]
    private extern static uint SetSystemTime(ref SystemTime lpSystemTime);


    public struct SystemTime
    {
      public ushort wYear;
      public ushort wMonth;
      public ushort wDayOfWeek;
      public ushort wDay;
      public ushort wHour;
      public ushort wMinute;
      public ushort wSecond;
      public ushort wMilliseconds;
    }

    public static SystemTime GetTime()
    {
      // Call the native GetSystemTime method 
      // with the defined structure.
      SystemTime stime = new SystemTime();
      GetSystemTime(ref stime);

      return stime;
    }

    public static void SetTime()
    {
      // Call the native GetSystemTime method 
      // with the defined structure.
      SystemTime systime = new SystemTime();
      GetSystemTime(ref systime);

      // Set the system clock ahead one hour.
      systime.wHour = (ushort)(systime.wHour + 1 % 24);
      SetSystemTime(ref systime);
      Console.WriteLine("New time: " + systime.wHour + ":"
          + systime.wMinute);
    }
  }
}
