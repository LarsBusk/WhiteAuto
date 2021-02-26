using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Ui;

namespace MM2RunCheckSamples
{
  class Program
  {

    static void Main(string[] args)
    {

      MeatMaster2Functions functions = new MeatMaster2Functions();

      int csCounter = 0;

      while (true)
      {
        csCounter++;
        functions.ClickStartStopButton();
        Functions.logger.LogInfo("Running CS no {0}", csCounter);

        functions.StartAndWaitForCheckSample(25);
      }
    }
  }
}
