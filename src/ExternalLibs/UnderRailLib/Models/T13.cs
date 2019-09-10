using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("T13")]
    [Serializable]
    public sealed class T13 : BaseAction
    {
        private T13(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ic = info.GetString("T13:IC");
            _s = info.GetInt32("T13:S");
            _i = info.GetBoolean("T13:I");
            _sg = info.GetBoolean("T13:SG");
            _n = (info.GetValue("T13:N", typeof(Guid?)) as Guid?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("T13:IC", _ic);
            info.AddValue("T13:S", _s);
            info.AddValue("T13:I", _i);
            info.AddValue("T13:SG", _sg);
            info.AddValue("T13:N", _n);
        }

        private string _ic;

        private int _s = 1;

        private bool _i;

        private bool _sg = true;

        private Guid? _n;
    }
}
