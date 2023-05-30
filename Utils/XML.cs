using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Examath.Core.Utils
{
    public static class XML
    {
        public static async Task<T?> TryLoad<T> (string fileLocation)
        {
            T? Data = default;

            try
            {
                Data = await Task.Run(() => Load(fileLocation));
            }
            catch (Exception)
            {

            }

            return Data;

            static T? Load(string fileLocation)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using FileStream fileStream = File.Open(fileLocation, FileMode.Open);
                using var reader = XmlReader.Create(fileStream);
                return (T?)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
