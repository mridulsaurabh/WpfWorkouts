using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Cerberus.ToBeUsed
{
    public class CustomXMLExtension : MarkupExtension
    {
        // How to use: ItemsSource="{local:CustomXMLExtension Source=Books.xml, Path=/Book/Title}"

        public string Source { get; set; }
        public string Path { get; set; }
        private static List<string> ParseXMLFileToItems(string file, string path)
        {
            XDocument xdoc = XDocument.Load(file);
            string[] text = path.Substring(1).Split('/');
            string desc = text[0].ToString();
            string elementname = text[1].ToString();
            List<string> data = new List<string>();
            IEnumerable<XElement> elems = xdoc.Descendants(desc);

            IEnumerable<XElement> elem_list = from elem in elems
                                              select elem;
            foreach (XElement element in elem_list)
            {
                String str0 =
                element.Attribute(elementname).Value.ToString();
                data.Add(str0);
            }
            return data;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if ((this.Source != null) && (this.Path != null))
                return ParseXMLFileToItems(Source, Path);
            else
                throw new InvalidOperationException("Inputs cannot be blank");
        }
    }
}
