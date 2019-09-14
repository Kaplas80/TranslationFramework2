using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

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
            using (var memoryStream = new MemoryStream())
            {
                SerializeToBinary(obj, binder, memoryStream, currentDataModelVersion, embedDataModelVersion);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
        }

        public static T DeserializeFromBinaryFile<T>(string filePath, SerializationBinder binder = null,
            bool tryToRetrieveDataModelVersion = false)
        {
            T result;
            using (var fileStream = File.OpenRead(filePath))
            {
                result = DeserializeFromBinary<T>(fileStream, binder, tryToRetrieveDataModelVersion);
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

        public static T DeserializeFromBinaryFileCompressed<T>(string filePath, SerializationBinder binder = null, bool tryToRetrieveDataModelVersion = false)
        {
            T result;
            using (var fileStream = File.OpenRead(filePath))
            {
                var dataModelVersion = 0L;
                if (tryToRetrieveDataModelVersion)
                {
                    dataModelVersion = RetrieveDataModelVersion(fileStream);
                }

                using (var ms = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    {
                        var buffer = new byte[4096];
                        int count;
                        while ((count = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, count);
                        }
                    }

                    ms.Position = 0L;
                    var t = DeserializeFromBinaryInternal<T>(ms, binder, dataModelVersion);
                    result = t;
                }
            }
            return result;
        }

        public static void SerializeToBinaryFileCompressed<T>(string filePath, T obj, SerializationBinder binder = null,
            long currentDataModelVersion = 0L, bool embedDataModelVersion = false)
        {
            using (var fileStream = File.Create(filePath, 16384))
            {
                if (embedDataModelVersion)
                {
                    EmbedDataModelVersion(fileStream, currentDataModelVersion);
                }

                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                {
                    SerializeToBinaryInternal(gzipStream, obj, binder, currentDataModelVersion);
                }
                
                //gzipStream.Flush();
                //gzipStream.Close();
            }
        }

        public static readonly Guid Id = new Guid("{838B53F9-361F-4332-BAAE-0D17865D0854}");
        public static readonly byte[] IdByteArray = Id.ToByteArray();
    }
}
