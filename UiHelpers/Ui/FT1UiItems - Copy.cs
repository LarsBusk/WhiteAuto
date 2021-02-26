using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.Finders;

namespace Common.Ui
{
  public class FT1UiItems
  {
    public const string Ft1ProcessName = "MscFT1";

    public const string MainWindowName = "MSC FT1 - [Results]";

    public const string MenuBarAutomationId = "MenuBar";

    public const string MirrorGainDialogTitle = "Mirror/Gain Adjustment";

    public const string DelayAdjustmentTitle = "Delay Adjustment";

    public const string NoiseTestTitle = "Noise Test";

    public const string FilterTestTitle = "Filter Test";

    public static SearchCriteria ShortScanRadioButton = SearchCriteria.ByText("Short Scan");

    public static SearchCriteria StoreButton = SearchCriteria.ByText("Store");

    public static SearchCriteria StartButton = SearchCriteria.ByText("Start");

    public static SearchCriteria ExitButton = SearchCriteria.ByText("Exit");
  }
}
