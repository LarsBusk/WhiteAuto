using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
  public static class DatabaseHelpers
  {
    private static readonly Logger logger = new Logger();

    /// <summary>
    /// Deletes the LastDataMaintenance setting from the tblNovaSystemSetting table
    /// </summary>
    public static void DeleteLastDataMaintenanceDate()
    {
      try
      {
        SqlConnection sqlConnection1 =
          new SqlConnection(@"Server=.\SQLEXPRESS;Initial Catalog=MeatMasterII;Integrated Security=true;");
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText = "DELETE FROM tblNovaSystemSetting WHERE	SettingName = 'LastDataMaintenance'";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        cmd.ExecuteReader();

        sqlConnection1.Close();
      }
      catch (Exception e)
      {
        logger.LogError("Failed to delete last datamaintenance with exception {0}", e.Message);
      }
    }
  }
}
