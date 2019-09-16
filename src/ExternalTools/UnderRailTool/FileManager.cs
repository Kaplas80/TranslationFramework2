using System;
using System.Diagnostics;
using System.IO;

namespace UnderRailTool
{
    public static class FileManager
    {
        public static T Load<T>(string path, bool isCompressed)
        {
            if (File.Exists(path))
            {
                try
                {
                    if (!isCompressed)
                    {
                        return dsj.DeserializeFromBinaryFile<T>(path, co0.a(), true);
                    }
                    else
                    {
                        return dsj.DeserializeFromBinaryFileCompressed<T>(path, co0.a(), true);
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
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            try
            {
                if (!isCompressed)
                {
                    dsj.SerializeToBinaryFile(path, model, co0.a(), dm3.b(), true);
                }
                else
                {
                    dsj.SerializeToBinaryFileCompressed(path, model, co0.a(), dm3.b(), true);
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
            var directory = Path.GetDirectoryName(outputPath);
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
