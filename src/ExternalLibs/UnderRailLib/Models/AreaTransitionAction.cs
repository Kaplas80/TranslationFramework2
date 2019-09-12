using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ATA")]
    [Serializable]
    public sealed class AreaTransitionAction : BaseAction
    {
        private AreaTransitionAction(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _destinationWaypoint = A_0.GetString("ATA:DW");
                return;
            }
            if (GetType() == typeof(AreaTransitionAction))
            {
                _destinationWaypoint = A_0.GetString("_DestinationWaypoint");
                return;
            }
            _destinationWaypoint = A_0.GetString("AreaTransitionAction+_DestinationWaypoint");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ATA:DW", _destinationWaypoint);
        }

        private string _destinationWaypoint;
    }
}
