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
    /// <summary>
    /// Represents a ViewModel that can be loaded from and saved to an XML file of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of model</typeparam>
    public abstract class XMLFileObject<T> : FileManipulationObject
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fileFilter"><inheritdoc/></param>
        public XMLFileObject(params FileFilter[] fileFilter) : base(fileFilter)
        {

        }

        private XmlSerializer _XmlSerializer = new(typeof(T));

        /// <summary>
        /// Gets or sets the settings used for the <see cref="XmlWriter"/>
        /// </summary>
        public XmlWriterSettings XmlWriterSettings { get; set; } = new()
        {
            Indent = true,
            Async = true,
        };

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
        /// Deserializes XML data from the file at <see cref="FileLocation"/> to <see cref="Data"/>.
        /// </summary>
        public override void LoadFile()
        {
            if (FileLocation != null)
            {
                using FileStream fileStream = File.Open(FileLocation, FileMode.Open);
                using var reader = XmlReader.Create(fileStream);
                Data = (T?)_XmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserializes XML data from the file at <see cref="FileLocation"/> to <see cref="Data"/> asynchronously.
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
        /// Serialises <see cref="Data"/> to the file at <see cref="FileLocation"/>
        /// </summary>
        public override void SaveFile()
        {
            if (FileLocation != null)
            {
                using FileStream fileStream = File.Create(FileLocation);
                using var writer = XmlWriter.Create(fileStream, XmlWriterSettings);
                _XmlSerializer.Serialize(writer, Data);
            }
        }

        /// <summary>
        /// Serialises <see cref="Data"/> to the file at <see cref="FileLocation"/> asynchronously
        /// </summary>
        /// <returns></returns>
        public override async Task SaveFileAsync()
        {
            await Task.Run(SaveFile);
        }
    }
}
