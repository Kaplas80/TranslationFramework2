using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M26")]
    [Serializable]
    public sealed class M26 : ModifyStatPercentageEffect
    {
        private M26(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _school = (info.GetValue("M26:S", typeof(PsiSchool?)) as PsiSchool?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M26:S", _school);
        }

        private PsiSchool? _school;
    }
}
