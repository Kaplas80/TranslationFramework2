using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OCUEA")]
    [Serializable]
    public class OCUEA : UEA
    {
        protected OCUEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _opened = info.GetBoolean("OCUEA:O");
                _simpleStates = info.GetBoolean("OCUEA:SS");
                _openedStateFrame = (int?)info.GetValue("OCUEA:OSF", typeof(int?));
                _closedStateFrame = (int?)info.GetValue("OCUEA:CSF", typeof(int?));
                SerializationHelper.ReadList("OCUEA:OOJ", ref _onOpenedJobs, info);
                SerializationHelper.ReadList("OCUEA:OCJ", ref _onClosedJobs, info);
                if (DataModelVersion.MinorVersion >= 8)
                {
                    _os = info.GetString("OCUEA:OS");
                    _cs = info.GetString("OCUEA:CS");
                }
                if (DataModelVersion.MinorVersion >= 347)
                {
                    SerializationHelper.ReadList("OCUEA:OOFT", ref _ooft, info);
                    _fto = info.GetBoolean("OCUEA:FTO");
                }
            }
            else
            {
                _opened = info.GetBoolean("_Opened");
                _simpleStates = info.GetBoolean("_SimpleStates");
                _openedStateFrame = (info.GetValue("_OpenedStateFrame", typeof(int?)) as int?);
                _closedStateFrame = (info.GetValue("_ClosedStateFrame", typeof(int?)) as int?);
                try
                {
                    SerializationHelper.ReadList("_OnOpenedJobs", ref _onOpenedJobs, info);
                    SerializationHelper.ReadList("_OnClosedJobs", ref _onClosedJobs, info);
                }
                catch
                {
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OCUEA:O", _opened);
            info.AddValue("OCUEA:SS", _simpleStates);
            info.AddValue("OCUEA:OSF", _openedStateFrame);
            info.AddValue("OCUEA:CSF", _closedStateFrame);
            SerializationHelper.WriteList("OCUEA:OOJ", _onOpenedJobs, info);
            SerializationHelper.WriteList("OCUEA:OCJ", _onClosedJobs, info);
            SerializationHelper.WriteList("OCUEA:OOFT", _ooft, info);
            info.AddValue("OCUEA:OS", _os);
            info.AddValue("OCUEA:CS", _cs);
            info.AddValue("OCUEA:FTO", _fto);
        }

        private bool _opened;

        private bool _simpleStates;

        private int? _openedStateFrame;

        private int? _closedStateFrame;

        private List<Job> _onOpenedJobs = new List<Job>();

        private List<Job> _onClosedJobs = new List<Job>();

        private List<Job> _ooft = new List<Job>();

        private string _os;

        private string _cs;

        private bool _fto;
    }
}
