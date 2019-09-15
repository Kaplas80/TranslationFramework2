using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MASE")]
    [Serializable]
    public sealed class MASE : StatusEffect
    {
        private MASE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetSingle("MASE:A");
            _afw = (HashSet<WeaponType>)info.GetValue("MASE:AFW", typeof(HashSet<WeaponType>));
            _p = info.GetInt32("MASE:P");
            if (DataModelVersion.MinorVersion >= 61)
            {
                _c = info.GetInt32("MASE:C");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MASE:A", _a);
            info.AddValue("MASE:AFW", _afw);
            info.AddValue("MASE:P", _p);
            info.AddValue("MASE:C", _c);
        }

        private float _a;

        private HashSet<WeaponType> _afw = new HashSet<WeaponType>();

        private int _p;

        private int _c;
    }
}
