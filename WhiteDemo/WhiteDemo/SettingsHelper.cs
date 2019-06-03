using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace WhiteDemo
{
  class SettingsHelper
  {
    private static List<string> logList;
    private static Window mainWindow;
    public void LogSettingsNames()
    {
      logList = new List<string>();


      Application fi = Application.Attach(Process.GetProcessesByName("WSF").FirstOrDefault());
      //Log($"Program found {fi.Name}");
      List<Window> fiWindows = fi.GetWindows();

      mainWindow = fiWindows.FirstOrDefault(w => w.Title.StartsWith("Foss Integrator"));
      //Log($"Window '{mainWindow.Title}' is found.");

      List<Window> modals = mainWindow.ModalWindows();
      foreach (var window in modals)
      {
        //Log($"Modal window with title: {window.Title} found.");
      }

      Window settingsWindow = modals.Find(w => w.Title.Equals("Settings"));

      ListBox settingsList = settingsWindow.Get<ListBox>(SearchCriteria.ByAutomationId("59649"));
      //Log("Settings view list found, items:");
      ListItems settingsItems = settingsList.Items;

      foreach (var settingsItem in settingsItems)
      {
       // Log(settingsItem.Text);
      }
    }
  }
}
