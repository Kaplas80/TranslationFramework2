using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IVC")]
    [Serializable]
    public sealed class IVC : ISerializable
    {
        private IVC(SerializationInfo info, StreamingContext ctx)
        {
            _c = info.GetDouble("IVC:C");
            if (DataModelVersion.MinorVersion >= 402)
            {
                SerializationHelper.ReadList("IVC:E", ref _e, info);
                _d = info.GetDouble("IVC:D");
            }
            else
            {
                _e = new List<EntityReference>();
            }
            if (DataModelVersion.MinorVersion >= 403)
            {
                _l = info.GetDouble("IVC:L");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IVC:C", _c);
            SerializationHelper.WriteList("IVC:E", _e, info);
            info.AddValue("IVC:D", _d);
            info.AddValue("IVC:L", _l);
        }

        private double _c = 750.0;

        private List<EntityReference> _e = new List<EntityReference>();

        private double _d;

        private double _l;
    }
}
