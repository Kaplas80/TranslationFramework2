using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Events;

namespace UnderRailLib.Models
{
    [EncodedTypeName("L")]
    [Serializable]
    public sealed class Lantern : IRE, IEventSubscriber<eaNCEA>, iPEP, iIMP
    {
        private Lantern(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _definition = (LanternDefinition)info.GetValue("L:D", typeof(LanternDefinition));
                _oldSourceNode = (Point?)info.GetValue("L:OSN", typeof(Point?));
                return;
            }
            if (GetType() == typeof(Lantern))
            {
                _definition = (LanternDefinition)info.GetValue("_Definition", typeof(LanternDefinition));
                _oldSourceNode = (Point?)info.GetValue("_OldSourceNode", typeof(Point?));
                return;
            }
            _definition = (LanternDefinition)info.GetValue("Lantern+_Definition", typeof(LanternDefinition));
            _oldSourceNode = (Point?)info.GetValue("Lantern+_OldSourceNode", typeof(Point?));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("L:D", _definition);
            info.AddValue("L:OSN", _oldSourceNode);
        }

        private LanternDefinition _definition;

        private Point? _oldSourceNode;
    }
}
