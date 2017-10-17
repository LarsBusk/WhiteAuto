using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
  [Serializable]
  public class ProductList
  {
    public int SamplesPrProduct { get; set; }

    public int TotalSamples { get; set; }

    public int TimeBetweenSamples { get; set; }

    public List<Product> Products { get; set; }

    public ProductList()
    {
      this.Products = new List<Product>();
      this.SamplesPrProduct = 1;
      this.TotalSamples = 0;
      this.TimeBetweenSamples = 30;
    }

    public override string ToString()
    {
      StringBuilder builder = new StringBuilder();
      foreach (var product in Products)
      {
        builder.AppendFormat("Product name: {0}", product.Name);
      }

      builder.AppendFormat("Samples pr product: {0}", SamplesPrProduct);
      builder.AppendFormat("Total samples: {0}", TotalSamples);

      return builder.ToString();
    }
  }

  public class Product
  {
    public string Name { get; set; }

    public Product()
    {
      this.Name = "MyProduct";
    }

    public Product(string productName)
    {
      this.Name = productName;
    }

    
  }
}
