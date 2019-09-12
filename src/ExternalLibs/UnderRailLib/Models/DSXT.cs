using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DSXT")]
    [Serializable]
    public sealed class DSXT : Job
    {
        private DSXT(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("DSXT:I", typeof(Guid?));
            _a = info.GetString("DSXT:A");
            _f = info.GetString("DSXT:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DSXT:I", _i);
            info.AddValue("DSXT:A", _a);
            info.AddValue("DSXT:F", _f);
        }

        private Guid? _i;

        private string _a;

        private string _f;
    }
}
