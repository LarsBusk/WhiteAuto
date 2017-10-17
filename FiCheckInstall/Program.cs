using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utilities;

namespace FiCheckInstall
{
  class Program
  {
    static void Main(string[] args)
    {
      FiInstallationHelper helper = new FiInstallationHelper();

      helper.Install();
    }
  }
}
