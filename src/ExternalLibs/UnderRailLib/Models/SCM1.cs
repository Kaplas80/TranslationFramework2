using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCM1")]
    [Serializable]
    public sealed class SCM1 : Job
    {
        private SCM1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mc = info.GetString("SCM1:MC");
            _gender = (Gender?) info.GetValue("SCM1:G", typeof(Gender?));
            _ori = (Guid?) info.GetValue("SCM1:ORI", typeof(Guid?));
            _a = info.GetString("SCM1:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCM1:MC", _mc);
            info.AddValue("SCM1:G", _gender);
            info.AddValue("SCM1:ORI", _ori);
            info.AddValue("SCM1:A", _a);
        }

        private string _mc;

        private Gender? _gender;

        private Guid? _ori;

        private string _a;
    }
}