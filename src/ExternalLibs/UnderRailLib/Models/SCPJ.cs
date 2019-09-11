using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCPJ")]
    [Serializable]
    public sealed class SCPJ : Job
    {
        private SCPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = (info.GetValue("SCPJ:P", typeof(PropertyCollection)) as PropertyCollection);
            if (DataModelVersion.MinorVersion >= 254)
            {
                _i = (Guid?) info.GetValue("SCPJ:I", typeof(Guid?));
                _a = info.GetString("SCPJ:A");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCPJ:P", _p);
            info.AddValue("SCPJ:I", _i);
            info.AddValue("SCPJ:A", _a);
        }

        private Guid? _i;

        private string _a;

        private PropertyCollection _p = new PropertyCollection();
    }
}
