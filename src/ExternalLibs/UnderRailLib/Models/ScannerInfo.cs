using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SI2")]
    [Serializable]
    public sealed class ScannerInfo : ISerializable
    {
        private ScannerInfo(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _scannerType = info.GetString("SI2:ST");
                _interval = info.GetInt32("SI2:I");
                return;
            }
            if (GetType() == typeof(ScannerInfo))
            {
                _scannerType = info.GetString("_ScannerType");
                _interval = info.GetInt32("_Interval");
                return;
            }
            _scannerType = info.GetString("ScannerInfo+_ScannerType");
            _interval = info.GetInt32("ScannerInfo+_Interval");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SI2:ST", _scannerType);
            info.AddValue("SI2:I", _interval);
        }

        private string _scannerType;

        private int _interval;
    }
}
