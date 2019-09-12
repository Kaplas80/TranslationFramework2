using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SIJ")]
    [Serializable]
    public sealed class StartInterfacingJob : Job
    {
        private StartInterfacingJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _dialog = info.GetString("SIJ:D");
                _overrideInterfaceActorName = info.GetString("SIJ:OIAN");
                _interfaceActorId = info.GetString("SIJ:IAI");
                _allowInCombat = info.GetBoolean("SIJ:AIC");
                _ft = DataModelVersion.MinorVersion < 36 || info.GetBoolean("SIJ:FT");

                if (DataModelVersion.MinorVersion >= 40)
                {
                    _upu = info.GetBoolean("SIJ:UPU");
                }

                if (DataModelVersion.MinorVersion >= 312)
                {
                    _o = (info.GetValue("SIJ:O", typeof(Guid?)) as Guid?);
                }
            }
            else
            {
                if (GetType() == typeof(StartInterfacingJob))
                {
                    _dialog = info.GetString("_Dialog");
                    _overrideInterfaceActorName = info.GetString("_OverrideInterfaceActorName");
                    _interfaceActorId = info.GetString("_InterfaceActorId");
                    _allowInCombat = info.GetBoolean("_AllowInCombat");
                    return;
                }

                _dialog = info.GetString("StartInterfacingJob+_Dialog");
                _overrideInterfaceActorName = info.GetString("StartInterfacingJob+_OverrideInterfaceActorName");
                _interfaceActorId = info.GetString("StartInterfacingJob+_InterfaceActorId");
                _allowInCombat = info.GetBoolean("StartInterfacingJob+_AllowInCombat");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SIJ:D", _dialog);
            info.AddValue("SIJ:OIAN", _overrideInterfaceActorName);
            info.AddValue("SIJ:IAI", _interfaceActorId);
            info.AddValue("SIJ:AIC", _allowInCombat);
            info.AddValue("SIJ:FT", _ft);
            info.AddValue("SIJ:UPU", _upu);
            info.AddValue("SIJ:O", _o);
        }

        private string _dialog;

        private string _overrideInterfaceActorName;

        private string _interfaceActorId = "interface";

        private bool _allowInCombat;

        private bool _ft = true;

        private bool _upu;

        private Guid? _o;
    }
}
