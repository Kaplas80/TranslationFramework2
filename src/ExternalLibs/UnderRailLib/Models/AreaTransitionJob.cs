using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ATJ")]
    [Serializable]
    public sealed class AreaTransitionJob : Job
    {
        private AreaTransitionJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _destinationWaypoint = info.GetString("ATJ:DW");
                _c = DataModelVersion.MinorVersion < 189 || info.GetBoolean("ATJ:C");

                if (DataModelVersion.MinorVersion >= 273)
                {
                    _i = (Guid?) info.GetValue("ATJ:I", typeof(Guid?));
                    _a = info.GetString("ATJ:A");
                }
            }
            else
            {
                if (GetType() == typeof(AreaTransitionJob))
                {
                    _destinationWaypoint = info.GetString("_DestinationWaypoint");
                    return;
                }

                _destinationWaypoint = info.GetString("AreaTransitionJob+_DestinationWaypoint");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ATJ:DW", _destinationWaypoint);
            info.AddValue("ATJ:C", _c);
            info.AddValue("ATJ:I", _i);
            info.AddValue("ATJ:A", _a);
        }

        private string _destinationWaypoint;

        private bool _c;

        private Guid? _i;

        private string _a;
    }
}
