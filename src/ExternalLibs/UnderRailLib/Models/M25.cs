using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M25")]
    [Serializable]
    public sealed class M25 : ModifyStatPercentageEffect
    {
        private M25(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 161)
            {
                _school = (info.GetValue("M25:S", typeof(PsiSchool?)) as PsiSchool?);
                return;
            }
            _school = (PsiSchool)info.GetValue("M25:S", typeof(PsiSchool));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);

            info.AddValue("M25:S", _school);
        }

        private PsiSchool? _school;
    }
}
