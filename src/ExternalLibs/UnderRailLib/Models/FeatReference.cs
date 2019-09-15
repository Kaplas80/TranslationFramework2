using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FR")]
    [Serializable]
    public sealed class FeatReference : TypedObjectReference<acd>
    {
        private FeatReference(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _fullTypeName = info.GetString("FR:FTN");
                return;
            }
            if (GetType() == typeof(FeatReference))
            {
                _fullTypeName = info.GetString("_FullTypeName");
                return;
            }
            _fullTypeName = info.GetString("FeatReference+_FullTypeName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("FR:FTN", _fullTypeName);
        }

        private string _fullTypeName;
    }
}
