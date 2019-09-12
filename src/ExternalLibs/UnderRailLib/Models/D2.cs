using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("D2")]
    [Serializable]
    public sealed class D2 : ISerializable
    {
        private D2(SerializationInfo A_0, StreamingContext A_1)
        {
            _code = A_0.GetString("D2:C");
            _name = A_0.GetString("D2:N");
            SerializationHelper.ReadList("D2:P", ref _parameters, A_0);
            _read = A_0.GetBoolean("D2:R");
            if (DataModelVersion.MinorVersion >= 21)
            {
                _portrait = A_0.GetString("D2:P");
                _author = A_0.GetString("D2:A");
                if (DataModelVersion.MinorVersion >= 49)
                {
                    SerializationHelper.ReadList("D2:OR", ref _onReadJobs, A_0);
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("D2:C", _code);
            info.AddValue("D2:N", _name);
            SerializationHelper.WriteList("D2:P", _parameters, info);
            SerializationHelper.WriteList("D2:OR", _onReadJobs, info);
            info.AddValue("D2:R", _read);
            info.AddValue("D2:P", _portrait);
            info.AddValue("D2:A", _author);
        }

        private string _code;

        private string _name;

        private List<string> _parameters = new List<string>();

        private bool _read;

        private string _portrait;

        private string _author;

        private List<string> _onReadJobs = new List<string>();
    }
}
