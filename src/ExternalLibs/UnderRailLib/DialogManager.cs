using System;
using System.Diagnostics;
using System.IO;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;

namespace UnderRailLib
{
    public class DialogManager
    {
        public DialogModel LoadModel(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    return SerializationManager.DeserializeFromBinaryFile<DialogModel>(path, Binder.Instance, true);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return null;
        }

        public void SaveModel(DialogModel dialogModel, string path)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            try
            {
                SerializationManager.SerializeToBinaryFile<DialogModel>(path, dialogModel, Binder.Instance, DataModelVersion.CurrentDataModelVersion, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}
