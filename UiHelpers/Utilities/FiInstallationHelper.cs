using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace Common.Utilities
{
  public class FiInstallationHelper
  {
    private string fiInstallerName = "FI-A.msi";


    public void Install(
      string computerName = "Local",
      string workStationName = "WorkStation",
      int workStationId = 1,
      bool emulator = false, 
      int dataBaseInitSize = 2000,
      int dataBaseGrow = 500,
      int dataBaseMaxSize = 10000,
      int dataBaseLogInitSize = 500,
      int dataBaseLogGrow = 500,
      int dataBaseLogMaxSize = 10000,
      string applicationFolder = "FossIntegrator")
    {
      StringBuilder argBuilder = new StringBuilder();

      if (computerName != "Local")
      {
        argBuilder.Append("COMPUTER_NAME " + computerName);
      }

      if (workStationName != "WorkStation")
      {
        argBuilder.Append("WORKSTATION_NAME " + workStationName);
      }

      if (workStationId != 1)
      {
        argBuilder.Append("WORKSTATION_ID " + workStationId);
      }

      if (dataBaseInitSize != 2000)
      {
        argBuilder.Append("DB_SIZE " + dataBaseInitSize);
      }

      if (dataBaseGrow != 500)
      {
        argBuilder.Append("DB_GROWTH " + dataBaseGrow);
      }

      if (dataBaseMaxSize != 10000)
      {
        argBuilder.Append("DB_MAX_SIZE" + dataBaseMaxSize);
      }

      if (dataBaseLogInitSize != 500)
      {
        argBuilder.Append("DB_LOG_SIZE" + dataBaseLogInitSize);
      }

      if (dataBaseLogGrow != 500)
      {
        argBuilder.Append("DB_GROWTH" + dataBaseLogGrow);
      }

      if (dataBaseLogMaxSize != 10000)
      {
        argBuilder.Append("DB_LOG_MAX_SIZE" + dataBaseLogMaxSize);
      }

      if (emulator)
      {
        argBuilder.Append("WORKSTATION_TYPE=Emulator,");
      }

      Installer.InstallProduct(fiInstallerName, argBuilder.ToString());
    }

    public void Uninstall()
    {
      Installer.InstallProduct(fiInstallerName, "Remove");
    }

  }
}
