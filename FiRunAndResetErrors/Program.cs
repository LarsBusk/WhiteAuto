using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;

namespace FiRunAndResetErrors
{
  class Program
  {
    static void Main(string[] args)
    {
      FiFunctions helpers = new FiFunctions();

      helpers.ChangeMode(FiFunctions.FiMode.Measuring);

      while (helpers.GetMode() != "Standby" || !helpers.StartButtonIsEnabled())
      {
        if (helpers.GetMode() == "Standby")
        {
          helpers.ResetErrors();

          helpers.ChangeMode(FiFunctions.FiMode.Measuring);
        }

        Thread.Sleep(TimeSpan.FromSeconds(30));
      }
    }
  } 
}
