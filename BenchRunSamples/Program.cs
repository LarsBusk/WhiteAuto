using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Common.Ui;
using Common.Utilities;

namespace BenchUIRunSamples
{
  class Program
  {
    static void Main(string[] args)
    {
      Uis myUi = new Uis();
      ApplicationHelpers applicationHelpers = new ApplicationHelpers();

      if (Uis.TryParse(args[0], out myUi))
      {
        Functions helper = new Functions(myUi);
        applicationHelpers.StartCaffeine();

        int sampleNo = 1;

        ProductList productList = helper.GetSamplesList(args);

        bool handleSampleReg = false;
        if (args.Count() > 2)
        {
          if (!bool.TryParse(args[2], out handleSampleReg))
          {
            handleSampleReg = false;
          }
        }

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

              if (handleSampleReg)
              {
                helper.HandleSampleRegistration();
              }


              Console.Clear();
              Console.WriteLine("Running Sample number {0}", sampleNo);

              helper.WaitForSample(productList.TimeBetweenSamples);

              sampleNo++;
            }
          }
        }
      }
      else
      {
        Console.WriteLine("{0} is not a valid UI, check command parameters", args[0]);
      }
    }
  }
}
