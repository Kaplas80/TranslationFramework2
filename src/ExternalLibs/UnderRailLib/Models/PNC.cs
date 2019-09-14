using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PNC")]
    [Serializable]
    public sealed class PNC : Condition
    {
        private PNC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetBoolean("PNC:I");
            _iwn = info.GetBoolean("PNC:IWN");
            _pn = info.GetString("PNC:PN");
            SerializationHelper.ReadList("PNC:PNL", ref _pnl, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PNC:I", _i);
            info.AddValue("PNC:IWN", _iwn);
            info.AddValue("PNC:PN", _pn);
            SerializationHelper.WriteList("PNC:PNL", _pnl, info);
        }

        private bool _i;

        private bool _iwn;

        private string _pn;

        private List<string> _pnl = new List<string>();
    }
}