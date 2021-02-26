using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Xml;
using System.Xml.Serialization;
using Castle.Core.Logging;
using Common.Ui;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public class Functions
  {
    #region private properties

    private string SelectProductsDialogName { get; set; }

    private SearchCriteria ProductList { get; set; }

    public ApplicationHelpers ApplicationHelpers { get; set; }

    public static Logger logger;

    private bool SelectProductDialogIsUpdated { get; set; }

    #endregion

    #region public properties

    public Window MainWindow { get; set; }

    public SearchCriteria StartStopButton { get; set; }

    #endregion

    #region public methods

    public Functions(Uis appToRun)
    {
      logger = new Logger();
      ApplicationHelpers = new ApplicationHelpers();

      logger.LogInfo($"Creating a new instance of functions for {appToRun}");

      switch (appToRun)
      {
        case Uis.MeatMaster2:
          this.MainWindow = ApplicationHelpers.GetMainWindow(MeatMaster2UiItems.Mm2ProcessName, MeatMaster2UiItems.Mm2MainWindowName);
          this.StartStopButton = MeatMaster2UiItems.Mm2StartStopButton;
          this.SelectProductsDialogName = UiItems.AdvancedSelectProductDialogName;
          this.ProductList = UiItems.AdvancedProductsList;
          this.SelectProductDialogIsUpdated = false;
          break;
        case Uis.pNova:
          this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.ProFossProcessName, UiItems.PnovaMainWindowName);
          this.StartStopButton = UiItems.PnovaStartStopButton;
          break;
        case Uis.Advanced:
          this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.AdvancedUiProcessName, UiItems.BenchMainWindowName);
          this.StartStopButton = UiItems.AdvancedUiStartStopButton;
          this.SelectProductsDialogName = UiItems.AdvancedSelectProductDialogName;
          this.ProductList = UiItems.AdvancedProductsList;
          this.SelectProductDialogIsUpdated = true;
          break;
          case Uis.Metrohm:
          this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.MetrohmProcessName, UiItems.BenchMainWindowName);
          this.StartStopButton = UiItems.AdvancedUiStartStopButton;
          this.SelectProductsDialogName = UiItems.AdvancedSelectProductDialogName;
          this.ProductList = UiItems.AdvancedProductsList;
          this.SelectProductDialogIsUpdated = true;
          break;
        case Uis.Mini:
          this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.MiniUiProcessName, UiItems.BenchMainWindowName);
          this.StartStopButton = UiItems.MiniUiStartStopButton;
          this.SelectProductsDialogName = UiItems.MiniUiSelectProductsDialogName;
          this.ProductList = UiItems.MiniUiProductsList;
          this.SelectProductDialogIsUpdated = false;
          break;
        case Uis.Matilde:
          this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.MatildeProcessName, UiItems.PnovaMainWindowName);
          break;
        default:
          break;
      }
    }


    /// <summary>
    /// Envoked to click the start stop button in any Nova
    /// </summary>
    public void ClickStartStopButton()
    {
      logger.LogInfo("Clicking start/stop button");

      Button startStopButton = MainWindow.Get<Button>(StartStopButton);
      logger.LogInfo("Button found {0}", startStopButton.Name);

      if (WaitHelpers.WaitForEnabled(startStopButton, TimeSpan.FromMinutes(5)))
        startStopButton.Click();
    }


    public void HandleSampleRegistration()
    {
      if (WaitHelpers.WaitForDialog(MainWindow, "UdfDialog", TimeSpan.FromSeconds(30)))
      {
        Button okButton = MainWindow.ModalWindow("UdfDialog").Get<Button>(SearchCriteria.ByText("PopupLeftButton"));
        WaitHelpers.WaitForEnabled(okButton, TimeSpan.FromMinutes(2));
        okButton.Click();
      }
    }

    /// <summary>
    /// Waits for the sample to complete in Mini or Advanced UI
    /// </summary>
    /// <param name="timeBetweenSamples"></param>
    public void WaitForSample(int timeBetweenSamples)
    {
      Button selectProductButton = MainWindow.Get<Button>(SearchCriteria.ByText(UiItems.SelectProductButtonName));
      
      WaitHelpers.WaitForEnabled(selectProductButton, TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(timeBetweenSamples));

      Thread.Sleep(TimeSpan.FromSeconds(timeBetweenSamples));
    }

    /// <summary>
    /// Selects a product in any Nova
    /// </summary>
    /// <param name="productName"></param>
    public void SelectProduct(string productName)
    {
      ClickSelectProduct();

      if (WaitHelpers.WaitForDialog(MainWindow, SelectProductsDialogName, TimeSpan.FromSeconds(30)))
      {
        Window selectProductDialog = MainWindow.ModalWindow(SelectProductsDialogName);

        if (SelectProductDialogIsUpdated) //True from 7.0
        {
          ClickButton(SearchCriteria.ByText(productName), selectProductDialog);
        }
        else
        {
          ListBox productList = selectProductDialog.Get<ListBox>(ProductList);
          SelectValueFromList(productName, productList);
        }
      }
    }

    public void HandlePopup(string answer)
    {
      WaitHelpers.WaitForDialog(MainWindow, UiItems.MessageBoxPopup, TimeSpan.FromSeconds(30));
      Window messagePopup = MainWindow.ModalWindow(UiItems.MessageBoxPopup);
      answer = answer.ToLower();

      if (answer == "yes")
      {
        ClickButton(UiItems.MessageBoxPopupYesButton, messagePopup);
        return;
      }

      ClickButton(UiItems.MessageBoxPopupNoButton, messagePopup);
      
      WaitHelpers.WaitFor(() => messagePopup.IsClosed, TimeSpan.FromSeconds(30));
    }

    public ProductList GetSamplesList(string[] args)
    {
      ProductList productList;

      if (args.Count() > 1)
      {
        using (XmlReader reader = XmlReader.Create(args[1]))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(ProductList));
          productList = (ProductList)serializer.Deserialize(reader);
        }
      }
      else
      {
        productList = new ProductList();
        productList.Products.Add(new Product(string.Empty));
      }

      return productList;
    }

    public void CloseDown()
    {
      ClickRadioButton(MeatMaster2UiItems.CareViewButton, MainWindow);
      Button closeButton = MainWindow.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.CloseDownButton));
      WaitHelpers.WaitForEnabled(closeButton, TimeSpan.FromSeconds(30));
      
      closeButton.Click();

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.MessageBoxPopup, TimeSpan.FromMinutes(2));
      
      MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup).Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupLeftButton)).Click();
    }

    

    public bool IsStopped()
    {
      Label stateLabel = MainWindow.Get<Label>(SearchCriteria.ByAutomationId(UiItems.MenuBarInstrumentStateText));

      logger.LogInfo("Instrument State is {0}", stateLabel.Name);
      return stateLabel.Name == "Stopped";
    }

    public void WaitForStopped()
    {
      while (!IsStopped())
      {
        Thread.Sleep(TimeSpan.FromSeconds(30));
      }
    }

    public void SelectInstrument(string serialNumber)
    {
      ComboBox selectInstrumentComboBox =
        MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId(UiItems.InstrumentListComboBox));
      try
      {
        selectInstrumentComboBox.Click();
        ListItem itemToSelect = selectInstrumentComboBox.Items.FirstOrDefault(i => i.Name.StartsWith(serialNumber));
        itemToSelect.Select();
      }
      catch (Exception e)
      {
        logger.LogError("SerialNumber {0} could not be selected. {1}", serialNumber, e.Message);
      }
    }

    public void ClickButton(SearchCriteria criteria, Window window)
    {
      logger.LogInfo($"Searching for button with criteria {criteria}");

      Button button = window.Get<Button>(criteria);
      logger.LogInfo($"Button found {button.Name} in {window.Title}");

      if (WaitHelpers.WaitForEnabled(button, TimeSpan.FromMinutes(5)))
      {
        logger.LogInfo($"Clicking {button.Name}");
        button.Click();
      }
    }

    public void ClickRadioButton(SearchCriteria criteria, Window window)
    {
      logger.LogInfo($"Searching for radiobutton with criteria {criteria}");

      RadioButton radioButton = window.Get<RadioButton>(criteria);
      logger.LogInfo($"Button found {radioButton.Name} in {window.Title}");

      if (WaitHelpers.WaitForEnabled(radioButton, TimeSpan.FromMinutes(5)))
      {
        logger.LogInfo($"Clicking {radioButton.Name}");
        radioButton.Click();
      }
    }

    public void ClickButton(SearchCriteria criteria)
    {
      ClickButton(criteria, MainWindow);
    }

    #endregion

    #region Private Methods

    private void SelectValueFromList(string itemToSelect, ListBox list)
    {
      ListItems items = list.Items;

      ListItem item = items.Find(
        x => x.GetElement(SearchCriteria.ByControlType(ControlType.Text)).
          GetCurrentPropertyValue(AutomationElement.NameProperty).Equals(itemToSelect));
      //Assert.NotNull(item, "The item {0} is not found on the list {1} items found.", itemToSelect, items.Count);
      item.Click();

    }

    private void ClickSelectProduct()
    {
      Button selectPruductButton = MainWindow.Get<Button>(SearchCriteria.ByText(UiItems.SelectProductButtonName));

      if (WaitHelpers.WaitForEnabled(selectPruductButton, TimeSpan.FromMinutes(5)))
      {
        selectPruductButton.Click();
      }
    }

    #endregion Private Methods

  }
}
