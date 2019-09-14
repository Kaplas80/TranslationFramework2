using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OSGJ")]
    [Serializable]
    public sealed class OperateSplitGateJob : Job
    {
        private OperateSplitGateJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _leftSegmentEntityId = (Guid) info.GetValue("OSGJ:LSEI", typeof(Guid));
                _rightSegmentEntityId = (Guid) info.GetValue("OSGJ:RSEI", typeof(Guid));
                _areaName = info.GetString("OSGJ:AN");
                _operation = (SplitGateOperation) info.GetValue("OSGJ:O", typeof(SplitGateOperation));
                if (DataModelVersion.MinorVersion >= 344)
                {
                    _tf = (info.GetValue("OSGJ:TF", typeof(int?)) as int?);
                    _as = (info.GetValue("OSGJ:AS", typeof(int?)) as int?);
                    _os = info.GetString("OSGJ:OS");
                    _cs = info.GetString("OSGJ:CS");
                }

                if (DataModelVersion.MinorVersion >= 439)
                {
                    _e = info.GetBoolean("OSGJ:E");
                }
            }
            else
            {
                if (GetType() == typeof(OperateSplitGateJob))
                {
                    _leftSegmentEntityId = (Guid) info.GetValue("_LeftSegmentEntityId", typeof(Guid));
                    _rightSegmentEntityId = (Guid) info.GetValue("_RightSegmentEntityId", typeof(Guid));
                    _areaName = info.GetString("_AreaName");
                    _operation = (SplitGateOperation) info.GetValue("_Operation", typeof(SplitGateOperation));
                    return;
                }

                _leftSegmentEntityId = (Guid) info.GetValue("OperateSplitGateJob+_LeftSegmentEntityId", typeof(Guid));
                _rightSegmentEntityId = (Guid) info.GetValue("OperateSplitGateJob+_RightSegmentEntityId", typeof(Guid));
                _areaName = info.GetString("OperateSplitGateJob+_AreaName");
                _operation = (SplitGateOperation) info.GetValue("OperateSplitGateJob+_Operation", typeof(SplitGateOperation));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OSGJ:LSEI", _leftSegmentEntityId);
            info.AddValue("OSGJ:RSEI", _rightSegmentEntityId);
            info.AddValue("OSGJ:AN", _areaName);
            info.AddValue("OSGJ:O", _operation);
            if (DataModelVersion.MinorVersion >= 344)
            {
                info.AddValue("OSGJ:TF", _tf);
                info.AddValue("OSGJ:AS", _as);
                info.AddValue("OSGJ:OS", _os);
                info.AddValue("OSGJ:CS", _cs);
            }

            if (DataModelVersion.MinorVersion >= 439)
            {
                info.AddValue("OSGJ:E", _e);
            }
        }

        private Guid _leftSegmentEntityId;

        private Guid _rightSegmentEntityId;

        private string _areaName;

        private SplitGateOperation _operation;

        private int? _tf;

        private int? _as;

        private string _os;

        private string _cs;

        private bool _e;
    }
}
