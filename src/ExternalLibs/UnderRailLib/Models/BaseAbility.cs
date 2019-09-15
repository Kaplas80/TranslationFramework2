using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BA2")]
    [Serializable]
    public abstract class BaseAbility : Stat<int>
    {
        protected BaseAbility(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("BA2:N");
                return;
            }
            if (GetType() == typeof(BaseAbility))
            {
                _name = info.GetString("_Name");
                return;
            }
            _name = info.GetString("BaseAbility+_Name");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("BA2:N", _name);
        }

        private string _name;
    }
}
