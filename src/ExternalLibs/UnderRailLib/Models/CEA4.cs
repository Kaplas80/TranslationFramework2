using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CEA4")]
    [Serializable]
    public class CEA4 : Aspect<LE2>, iLSTA, iINA, iOFBEA, ISerializable
    {
        protected CEA4(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("CEA4:C", ref _components, info);
                return;
            }
            SerializationHelper.ReadList("Components", ref _components, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("CEA4:C", _components, info);
        }

        public List<SEA> _components = new List<SEA>();
    }
}
