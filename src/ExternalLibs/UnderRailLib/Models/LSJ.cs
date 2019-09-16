using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LSJ")]
    [Serializable]
    public abstract class LSJ : Job
    {
        protected LSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _as = (eSPCJBT) info.GetValue("LSJ:AS", typeof(eSPCJBT));
            _ci = info.GetString("LSJ:CI");
            _si = info.GetString("LSJ:SI");
            _v = info.GetSingle("LSJ:V");
            _soundType = (SoundType) info.GetValue("LSJ:ST", typeof(SoundType));
            _l = info.GetBoolean("LSJ:L");
            _p = info.GetBoolean("LSJ:P");
            if (DataModelVersion.MinorVersion >= 14)
            {
                _oasei = (info.GetValue("LSJ:OASEI", typeof(Guid?)) as Guid?);
                _oasa = info.GetString("LSJ:OASA");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("LSJ:AS", _as);
            info.AddValue("LSJ:CI", _ci);
            info.AddValue("LSJ:SI", _si);
            info.AddValue("LSJ:V", _v);
            info.AddValue("LSJ:ST", _soundType);
            info.AddValue("LSJ:L", _l);
            info.AddValue("LSJ:P", _p);
            info.AddValue("LSJ:OASEI", _oasei);
            info.AddValue("LSJ:OASA", _oasa);
        }

        private eSPCJBT _as = eSPCJBT.c;

        private string _ci;

        private string _si;

        private float _v = 1f;

        private SoundType _soundType;

        private bool _l;

        private bool _p;

        private Guid? _oasei;

        private string _oasa;
    }
}
