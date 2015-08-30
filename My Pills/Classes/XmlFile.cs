using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace My_Pills.Classes
{
    public class XmlFile
    {
        private static StorageFolder _appFolder = ApplicationData.Current.LocalFolder;
        private static string _xmlFileName = "pills.xml";

        public static XElement Default
        {
            get
            {
                XElement xFile = new XElement("pills",                     
                    new XElement("time", new XAttribute("name", "Утро"))
                );

                return xFile;
            }
        }

        public static async Task Save(XElement xml)
        {
            StorageFile file = await _appFolder.CreateFileAsync(_xmlFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, xml.ToString());
        }

        public static async Task<bool> IsExists()
        {
            try
            {
                StorageFile file = await _appFolder.GetFileAsync(_xmlFileName);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
