using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utilities;

namespace FT1ServiceFunctions
{
  class Program
  {
    static void Main(string[] args)
    {
      FT1Functions functions = new FT1Functions();

      while (true)
      {
        functions.RunMirrorGainAdjustment();
        WaitHelpers.WaitSeconds(30);
        functions.RunDelayAdjustment();
        WaitHelpers.WaitSeconds(30);
        functions.RunFilterTest();
        WaitHelpers.WaitSeconds(30);
      }
    }
  }
}
