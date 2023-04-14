using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Examath.Core.Model
{
    public abstract class XMLFileObject<T> : FileManipulationObject
    {
        public XMLFileObject(params FileFilter[] fileFilter) : base(fileFilter)
        {

        }

        private XmlSerializer _XmlSerializer = new(typeof(T));

        private T? _Data;
        /// <summary>
        /// Gets or sets the data this <see cref="FileManipulationObject"/> holds
        /// </summary>
        public T? Data
        {
            get => _Data;
            set { if (SetProperty(ref _Data, value)) InitialiseData(); }
        }

        /// <summary>
        /// Called when <see cref="Data"/> is changed
        /// </summary>
        public virtual void InitialiseData()
        {

        }

        /// <summary>
        ///  Deserializes XML data from the file at <see cref="FileLocation"/> to <see cref="Data"/>
        /// </summary>
        public override async Task LoadFileAsync()
        {
            if (FileLocation != null)
            {
                using FileStream fileStream = File.Open(FileLocation, FileMode.Open);
                using var reader = XmlReader.Create(fileStream);
                await Task.Run(() => Data =  (T?)_XmlSerializer.Deserialize(reader));
            }
        }

        /// <summary>
        /// Serialiases <see cref="Data"/> to the file at <see cref="FileLocation"/>
        /// </summary>
        /// <returns></returns>
        public override async Task SaveFileAsync()
        {
            if (FileLocation != null) await Task.Run(() =>
            {
                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    Async = true,
                };

                using FileStream fileStream = File.Create(FileLocation);
                using var writer = XmlWriter.Create(fileStream, settings);
                _XmlSerializer.Serialize(writer, Data);
            });
        }
    }
}
