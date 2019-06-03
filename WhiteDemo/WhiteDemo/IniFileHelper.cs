using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace WhiteDemo
{
  public class IniFileHelper
  {
    private List<string> ReadIniFile(string name)
    {
      List<string> lines = new EditableList<string>();

      if (File.Exists(name))
      {
        lines = File.ReadAllLines(name).ToList();
      }

      return lines;
    }

    public void FindSection()
    {
      if (File.Exists("Custom.ini"))
      {
        List<string> lines = File.ReadAllLines("Custom.ini").ToList();
        int sectionStart = lines.IndexOf("[0x2700]");

        if (sectionStart != -1)
        {
          lines.Insert(sectionStart + 1, "ConfigurationDone=1");
          File.WriteAllLines("Custom.ini", lines);
        }
      }
    }

  }
}
