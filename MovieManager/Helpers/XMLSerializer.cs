using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MovieManager.Helpers
{
    public class XMLSerializer<T>
    {
        private string filePath;

        public XMLSerializer(string filePath)
        {
            this.filePath = filePath;
        }

        public T Deserialize()
        {
            if (string.IsNullOrEmpty(filePath))
                throw new InvalidOperationException("Invalid file path.");
            
            var serializer = new XmlSerializer(typeof(T));
            var streamReader = new StreamReader(filePath);

            var outputObject = (T)serializer.Deserialize(streamReader);

            streamReader.Close();

            return outputObject;
        }

        public void Serialize(T @object)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new InvalidOperationException("Invalid file path.");

            var fileStream = new FileStream(filePath, FileMode.Create);
            var writer = new XmlTextWriter(fileStream, Encoding.Unicode);

            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, @object);

            writer.Close();
        }
    }
}
