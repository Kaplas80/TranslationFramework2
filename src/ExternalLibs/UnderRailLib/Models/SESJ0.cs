using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SESJ0")]
    [Serializable]
    public sealed class SESJ0 : Job
    {
        private SESJ0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("SESJ0:I", typeof(Guid?));
            _a = info.GetString("SESJ0:A");
            _l = (Vector3)info.GetValue("SESJ0:L", typeof(Vector3));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SESJ0:I", _i);
            info.AddValue("SESJ0:A", _a);
            info.AddValue("SESJ0:L", _l);
        }

        private Vector3 _l;

        private Guid? _i;

        private string _a;
    }
}
