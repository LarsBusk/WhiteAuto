using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiHelpers;

namespace MiniUiRunSamples
{
  class Program
  {
    static void Main(string[] args)
    {
      Functions helper = new Functions(Uis.Mini);

      int sampleNo = 1;

      ProductList productList = helper.GetSamplesList(args);

      while (sampleNo < productList.TotalSamples || productList.TotalSamples == 0)
      {
        foreach (Product product in productList.Products)
        {
          if (!string.IsNullOrEmpty(product.Name))
          {
            helper.SelectProduct(product.Name);
          }

          for (int i = 0; i < productList.SamplesPrProduct; i++)
          {
            helper.ClickStartStopButton();

            helper.HandleSampleRegistration();

            Console.Clear();
            Console.WriteLine("Running Sample number {0}", sampleNo);

            helper.WaitForSample(productList.TimeBetweenSamples);

            sampleNo++;
          }
        }
      }
    }
  }
}
