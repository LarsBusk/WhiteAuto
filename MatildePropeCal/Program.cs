using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;

namespace MatildeMotorDrift
{
  class Program
  {
    static void Main(string[] args)
    {
      MatildeFunctions helper = new MatildeFunctions();

      string[] serialNumbers = { "Matilde PT3", "91740443" };

      while (true)
      {
        foreach (string serialNumber in serialNumbers)
        {
          helper.SelectInstrument(serialNumber);

          helper.ClickMotorDriftButton();

        helper.WaitForStopped();

        Thread.Sleep(TimeSpan.FromMinutes(2));
        }
        
      }
    }
  }
}
