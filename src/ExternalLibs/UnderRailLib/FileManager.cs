using System;
using System.Diagnostics;
using System.IO;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib
{
    public class FileManager
    {
        public T Load<T>(string path, bool isCompressed)
        {
            if (File.Exists(path))
            {
                try
                {
                    return !isCompressed ? SerializationManager.DeserializeFromBinaryFile<T>(path, Binder.Instance, true) : SerializationManager.DeserializeFromBinaryFileCompressed<T>(path, Binder.Instance, true);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return default;
        }

        public void Save<T>(T model, string path, bool isCompressed)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            try
            {
                if (!isCompressed)
                {
                    SerializationManager.SerializeToBinaryFile(path, model, Binder.Instance, DataModelVersion.CurrentDataModelVersion, true);
                }
                else
                {
                    SerializationManager.SerializeToBinaryFileCompressed(path, model, Binder.Instance, DataModelVersion.CurrentDataModelVersion, true);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void Dump(string inputPath, string outputPath)
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
                        Debug.WriteLine(e.Message);
                        //throw;
                    }
                }
            }
        }
    }
}
