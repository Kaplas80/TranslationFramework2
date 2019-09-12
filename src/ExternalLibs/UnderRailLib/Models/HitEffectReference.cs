using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("HER")]
    [Serializable]
    public sealed class HitEffectReference : ISerializable
    {
        private HitEffectReference(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _visible = info.GetBoolean("HER:V");
                _name = info.GetString("HER:N");
                _chance = DataModelVersion.MinorVersion >= 99 ? info.GetDouble("HER:C") : info.GetSingle("HER:C");

                _power = info.GetInt32("HER:P");
                return;
            }

            if (GetType() == typeof(HitEffectReference))
            {
                _visible = info.GetBoolean("_Visible");
                _name = info.GetString("_Name");
                _chance = info.GetSingle("_Chance");
                _power = info.GetInt32("_Power");
                return;
            }

            _visible = info.GetBoolean("HitEffectReference+_Visible");
            _name = info.GetString("HitEffectReference+_Name");
            _chance = info.GetSingle("HitEffectReference+_Chance");
            _power = info.GetInt32("HitEffectReference+_Power");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("HER:V", _visible);
            info.AddValue("HER:N", _name);
            info.AddValue("HER:C", _chance);
            info.AddValue("HER:P", _power);
        }

        private bool _visible = true;

        private string _name;

        private double _chance;

        private int _power;
    }
}
