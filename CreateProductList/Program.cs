using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Common.Utilities;

namespace CreateProductList
{
  class Program
  {
    static void Main(string[] args)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(ProductList));
      ProductList myList = new ProductList();

      using (XmlWriter writer = XmlWriter.Create("Plist.xml"))
      {
        serializer.Serialize(writer, myList);
        writer.Flush();
      }
    }
  }
}
