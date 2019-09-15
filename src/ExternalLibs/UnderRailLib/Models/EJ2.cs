using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EJ2")]
    [Serializable]
    public class EJ2 : Aspect<LE2>, iMAIA
    {
        protected EJ2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            SerializationHelper.ReadList("EJ2:J", ref _j, info);
            _e = info.GetInt32("EJ2:E");
            _m = info.GetInt32("EJ2:M");
            _d = info.GetInt32("EJ2:D");
            _p = info.GetInt32("EJ2:P");
            _c = (info.GetValue("EJ2:C", typeof(double?)) as double?);
            if (DataModelVersion.MinorVersion >= 502)
            {
                _x = (eSPCJBTL)info.GetValue("EJ2:X", typeof(eSPCJBTL));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("EJ2:J", _j, info);
            info.AddValue("EJ2:E", _e);
            info.AddValue("EJ2:M", _m);
            info.AddValue("EJ2:D", _d);
            info.AddValue("EJ2:P", _p);
            info.AddValue("EJ2:C", _c);
            info.AddValue("EJ2:X", _x);
        }

        private List<Job> _j = new List<Job>();

        private int _e;

        private int _m;

        private int _d;

        private int _p;

        private double? _c;

        private eSPCJBTL _x;
    }
}
