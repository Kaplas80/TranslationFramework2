using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("UHEJ")]
    [Serializable]
    public sealed class UncoverHiddenEntityJob : Job
    {
        private UncoverHiddenEntityJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _instant = info.GetBoolean("UHEJ:I");
                _area = info.GetString("UHEJ:A");
                _id = (Guid?) info.GetValue("UHEJ:I1", typeof(Guid?));
                return;
            }

            if (GetType() == typeof(UncoverHiddenEntityJob))
            {
                _instant = info.GetBoolean("_Instant");
                _area = info.GetString("_Area");
                _id = (Guid?) info.GetValue("_Id", typeof(Guid?));
                return;
            }

            _instant = info.GetBoolean("UncoverHiddenEntityJob+_Instant");
            _area = info.GetString("UncoverHiddenEntityJob+_Area");
            _id = (Guid?) info.GetValue("UncoverHiddenEntityJob+_Id", typeof(Guid?));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("UHEJ:I", _instant);
            info.AddValue("UHEJ:A", _area);
            info.AddValue("UHEJ:I1", _id);
        }

        private bool _instant = true;

        private string _area;

        private Guid? _id;
    }
}
