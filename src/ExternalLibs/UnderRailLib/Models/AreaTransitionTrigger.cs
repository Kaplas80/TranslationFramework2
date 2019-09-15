using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ATT")]
    [Serializable]
    public sealed class AreaTransitionTrigger : Trigger, ISerializable
    {
        private AreaTransitionTrigger(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _destinationWaypoint = info.GetString("ATT:DW");
                if (DataModelVersion.MinorVersion >= 472)
                {
                    _condition = (info.GetValue("ATT:C", typeof(iCOND)) as iCOND);
                }
            }
            else
            {
                if (GetType() == typeof(AreaTransitionTrigger))
                {
                    _destinationWaypoint = info.GetString("_DestinationWaypoint");
                    return;
                }
                _destinationWaypoint = info.GetString("AreaTransitionTrigger+_DestinationWaypoint");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ATT:DW", _destinationWaypoint);
            if (DataModelVersion.MinorVersion >= 472)
            {
                info.AddValue("ATT:C", _condition);
            }
        }
        private string _destinationWaypoint;

        private iCOND _condition;
    }
}
