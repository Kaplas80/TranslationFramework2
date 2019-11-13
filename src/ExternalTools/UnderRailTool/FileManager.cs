using System;
using System.Diagnostics;
using System.IO;

namespace UnderRailTool
{
    public static class FileManager
    {
        // Search for "DeserializeFromBinaryFile" in underrail.exe
        public static T Load<T>(string path, bool isCompressed)
        {
            if (File.Exists(path))
            {
                try
                {
                    if (!isCompressed)
                    {
                        return dt2.DeserializeFromBinaryFile<T>(path, cp3.a(), true);
                    }
                    else
                    {
                        return dt2.DeserializeFromBinaryFileCompressed<T>(path, cp3.a(), true);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return default;
        }

        public static void Save<T>(T model, string path, bool isCompressed)
        {
            string directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            try
            {
                if (!isCompressed)
                {
                    dt2.SerializeToBinaryFile(path, model, cp3.a(), doe.b(), true);
                }
                else
                {
                    dt2.SerializeToBinaryFileCompressed(path, model, cp3.a(), doe.b(), true);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public static void Dump(string inputPath, string outputPath)
        {
            string directory = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(directory);
            if (File.Exists(inputPath))
            {
                try
                {
                    SerializationManager.Dump(inputPath, outputPath, true);
                    
                }
                catch (Exception e)
                {
                    try
                    {
                        File.Copy(inputPath, outputPath, true);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                        //throw;
                    }
                }
            }
        }
    }
}
