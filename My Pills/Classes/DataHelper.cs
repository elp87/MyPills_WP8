using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace My_Pills.Classes
{
    class DataHelper
    {
        public static List<Pill> GetPillsFromXML(XElement xml, string periodName)
        {
            return xml.Descendants("time")
                                    .Where(x => x.Attribute("name").Value == periodName)
                                    .Descendants("item")
                                    .Select(s => new Pill(
                                        name: s.Element("name").Value,
                                        info: s.Element("info") != null ? s.Element("info").Value : "")
                                        )
                                    .ToList();
        }

        public static List<string> GetPeriodsFromXml(XElement xml)
        {
            return xml.Descendants("time").Select(s => s.Attribute("name").Value).ToList();
        }
    }
}
