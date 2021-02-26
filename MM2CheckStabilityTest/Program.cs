using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utilities;

namespace MM2CheckStabilityTest
{
  class Program
  {
    static void Main(string[] args)
    {
      MeatMaster2Functions functions = new MeatMaster2Functions();

      while (true)
      {
        functions.StartDiagnostics();
        functions.GetDiagnosticsResult();
        functions.SwitchOffXray();
      }
    }
  }
}
