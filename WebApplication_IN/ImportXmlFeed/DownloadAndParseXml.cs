using NuGet.Packaging;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebApplication_IN.Models;

namespace WebApplication_IN.ImportXmlFeed
{
    public class DownloadAndParseXml
    {
        public static List<Product> Parse_XML()
        {
            string fileName = @"E:\CSharpKurz\WebApplication_IN\WebApplication_IN\ImportXmlFeed\Feed.xml";

            XmlDocument _document = new XmlDocument();

            try
            {
                _document.Load(fileName);
            }
            catch (XmlException)
            {
                //neco vypsat
            }

            var doc = (XmlDocument)_document.CloneNode(true);

            //Get nodes to list
            XmlNodeList nodes = doc.GetElementsByTagName("Product");

            List<Product> listProduktu = new();

            foreach (XmlNode node in nodes)
            {
                listProduktu.Add(ConvertNode<Product>(node));
            }

            return listProduktu;
        }

        private static Product ConvertNode<T>(XmlNode node)
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(Product), new XmlRootAttribute("Product"));
            Product result = ser.Deserialize(stm) as Product;

            return result;
        }
    }
}

