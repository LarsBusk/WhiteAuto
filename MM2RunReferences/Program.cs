using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;

namespace MM2RunReferences
{
  class Program
  {
    static void Main(string[] args)
    {
      MeatMaster2Functions helper = new MeatMaster2Functions();
      Logger logger = new Logger();

      int i = 0;
      while (true)
      {
        i++;

        helper.ClickStartStopButton();

        helper.WaitForReference();
        
        logger.LogInfo($"Reference number {i} done.");

        Thread.Sleep(TimeSpan.FromMinutes(30));
      }
    }
  }
}
