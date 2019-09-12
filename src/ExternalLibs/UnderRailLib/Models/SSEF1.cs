using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SSEF1")]
    [Serializable]
    public sealed class btw : Job
    {
        private btw(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _id = (Guid?) info.GetValue("SSEF1:I", typeof(Guid?));
            _a = info.GetString("SSEF1:A");
            if (DataModelVersion.MinorVersion >= 233)
            {
                _f = (Point) info.GetValue("SSEF1:F", typeof(Point));
                _sf = info.GetBoolean("SSEF1:SF");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SSEF1:I", _id);
            info.AddValue("SSEF1:A", _a);
            info.AddValue("SSEF1:F", _f);
            info.AddValue("SSEF1:SF", _sf);
        }

        private Point _f;

        private bool _sf;

        private Guid? _id;

        private string _a;
    }
}