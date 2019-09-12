using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnderRailLib
{
    public static class SerializationManager
    {
        public static Stream SerializeToBinary<T>(T obj, SerializationBinder binder = null, Stream targetStream = null,
            long currentDataModelVersion = 0L, bool embedDataModelVersion = false,
            bool resetStreamPositionAtTheEnd = true)
        {
            var stream = targetStream ?? new MemoryStream();

            if (embedDataModelVersion)
            {
                EmbedDataModelVersion(stream, currentDataModelVersion);
            }

            SerializeToBinaryInternal(stream, obj, binder, currentDataModelVersion);
            if (resetStreamPositionAtTheEnd)
            {
                stream.Position = 0L;
            }

            return stream;
        }

        public static T DeserializeFromBinary<T>(Stream stream, SerializationBinder binder = null,
            bool tryToRetrieveDataModelVersion = false, bool resetStreamPositionFirst = true)
        {
            if (resetStreamPositionFirst)
            {
                stream.Position = 0L;
            }

            var dataModelVersion = 0L;
            if (tryToRetrieveDataModelVersion)
            {
                dataModelVersion = RetrieveDataModelVersion(stream);
            }

            return DeserializeFromBinaryInternal<T>(stream, binder, dataModelVersion);
        }

        public static void SerializeToBinaryFile<T>(string filePath, T obj, SerializationBinder binder = null,
            long currentDataModelVersion = 0L, bool embedDataModelVersion = false)
        {
            var memoryStream = new MemoryStream();
            try
            {
                SerializeToBinary(obj, binder, memoryStream, currentDataModelVersion, embedDataModelVersion);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
            finally
            {
                memoryStream.Close();
            }
        }

        public static T DeserializeFromBinaryFile<T>(string filePath, SerializationBinder binder = null,
            bool tryToRetrieveDataModelVersion = false)
        {
            FileStream fileStream = null;
            T result;
            try
            {
                fileStream = File.OpenRead(filePath);
                result = DeserializeFromBinary<T>(fileStream, binder, tryToRetrieveDataModelVersion);
            }
            finally
            {
                fileStream?.Close();
            }

            return result;
        }

        private static void EmbedDataModelVersion(Stream stream, long dataModelVersion)
        {
            stream.Write(IdByteArray, 0, IdByteArray.Length);
            stream.Write(BitConverter.GetBytes(dataModelVersion), 0, 8);
        }

        private static long RetrieveDataModelVersion(Stream stream)
        {
            if (stream.Length >= IdByteArray.Length)
            {
                var buffer = new byte[IdByteArray.Length];
                stream.Read(buffer, 0, IdByteArray.Length);
                var g = new Guid(buffer);
                if (Id.Equals(g))
                {
                    var array = new byte[8];
                    stream.Read(array, 0, 8);
                    return BitConverter.ToInt64(array, 0);
                }

                stream.Position = 0L;
            }

            return 0L;
        }

        private static void SerializeToBinaryInternal(Stream stream, object obj, SerializationBinder binder, long dataModelVersion)
        {
            DataModelVersion.CurrentDataModelVersion = dataModelVersion;
            var formatter = new BinaryFormatter
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple, 
                Binder = binder
            };
            formatter.Serialize(stream, obj);
        }

        private static T DeserializeFromBinaryInternal<T>(Stream stream, SerializationBinder binder,
            long dataModelVersion)
        {
            DataModelVersion.CurrentDataModelVersion = dataModelVersion;
            var formatter = new BinaryFormatter
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple, 
                Binder = binder
            };
            var t = (T) formatter.Deserialize(stream);
            return t;
        }

        public static readonly Guid Id = new Guid("{838B53F9-361F-4332-BAAE-0D17865D0854}");
        public static readonly byte[] IdByteArray = Id.ToByteArray();
    }
}
