using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Examath.Core.Properties;

namespace Examath.Core.Utils
{
    /// <summary>
    /// Utilities for loading XML files
    /// </summary>
    public static class XML
    {
        /// <summary>
        /// Calls <see cref="LoadAsync{T}(string)"/> within a try-catch block,
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="fileLocation">Location of xml file</param>
        /// <returns>The object, or default if an error occurred</returns>
        public static async Task<T?> TryLoadAsync<T> (string fileLocation)
        {
            try
            {
                return await LoadAsync<T>(fileLocation);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Deserialize an XML file to <typeparamref name="T"/> object asynchronously
        /// using a one-time <see cref="XmlSerializer"/>
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="fileLocation">Location of xml file</param>
        /// <returns>The object</returns>
        public static async Task<T?> LoadAsync<T>(string fileLocation)
        {
            return await Task.Run(() => Load(fileLocation));

            static T? Load(string fileLocation)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using FileStream fileStream = File.Open(fileLocation, FileMode.Open);
                using var reader = XmlReader.Create(fileStream);
                return (T?)xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize an object of type <typeparamref name="T"/> to an XML file asynchronously
        /// </summary>
        /// <typeparam name="T">The type to serialize from</typeparam>
        /// <param name="fileLocation">Location of xml file</param>
        /// <param name="data">The object to serialize</param>
        public static async Task SaveAsync<T> (string fileLocation, T data)
        {
            await Task.Run(() => Save(fileLocation, data));

            void Save(string fileLocation, T data)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using FileStream fileStream = File.Create(fileLocation);
                using var writer = XmlWriter.Create(fileStream);
                xmlSerializer.Serialize(writer, data);
            }
        }
    }
}
