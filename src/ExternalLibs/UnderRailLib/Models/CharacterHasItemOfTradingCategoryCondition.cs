using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CHIOTC")]
    [Serializable]
    public sealed class CharacterHasItemOfTradingCategoryCondition : Condition
    {
        private CharacterHasItemOfTradingCategoryCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ic = (eTC)info.GetValue("CHIOTC:IC", typeof(eTC));
            _cip = info.GetBoolean("CHIOTC:CIP");
            _id = (Guid?)info.GetValue("CHIOTC:ID", typeof(Guid?));
            _a = info.GetString("CHIOTC:A");
            _i = info.GetBoolean("CHIOTC:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CHIOTC:IC", _ic);
            info.AddValue("CHIOTC:CIP", _cip);
            info.AddValue("CHIOTC:ID", _id);
            info.AddValue("CHIOTC:A", _a);
            info.AddValue("CHIOTC:I", _i);
        }

        private eTC _ic;

        private bool _cip;

        private Guid? _id;

        private string _a;

        private bool _i;
    }
}
