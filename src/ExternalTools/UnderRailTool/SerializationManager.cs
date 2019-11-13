using System;
using System.IO;
using System.IO.Compression;

namespace UnderRailTool
{
    public static class SerializationManager
    {
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

        public static void Dump(string input, string output, bool tryToRetrieveDataModelVersion = false)
        {
            using (FileStream fileStream = File.OpenRead(input))
            using (FileStream fileStream2 = File.OpenWrite(output))
            {
                if (tryToRetrieveDataModelVersion)
                {
                    long dataModelVersion = RetrieveDataModelVersion(fileStream);
                    EmbedDataModelVersion(fileStream2, dataModelVersion);
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
                    ms.CopyTo(fileStream2);
                }
            }

        }

        public static readonly Guid Id = new Guid("{838B53F9-361F-4332-BAAE-0D17865D0854}");
        public static readonly byte[] IdByteArray = Id.ToByteArray();
    }
}
