using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSS")]
    [Serializable]
    public sealed class CharacterSpecialStates : ISerializable
    {
        private CharacterSpecialStates(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _stunned = info.GetBoolean("CSS:S");
                _immobilized = info.GetBoolean("CSS:I");
                _incapacitated = info.GetBoolean("CSS:I1");
                _mindBlocked = info.GetBoolean("CSS:MB");
                _jammed = info.GetBoolean("CSS:J");
                _handCuffed = info.GetBoolean("CSS:HC");
                _stealthed = info.GetBoolean("CSS:S1");
                if (DataModelVersion.MinorVersion >= 43)
                {
                    _f = info.GetBoolean("CSS:F");
                }

                if (DataModelVersion.MinorVersion >= 77)
                {
                    _p = info.GetBoolean("CSS:P");
                }

                if (DataModelVersion.MinorVersion >= 278)
                {
                    _n = info.GetBoolean("CSS:N");
                }

                if (DataModelVersion.MinorVersion >= 345)
                {
                    _t = info.GetBoolean("CSS:T");
                }

                if (DataModelVersion.MinorVersion >= 539)
                {
                    _r = info.GetBoolean("CSS:R");
                }
            }
            else
            {
                if (GetType() == typeof(CharacterSpecialStates))
                {
                    _stunned = info.GetBoolean("Stunned");
                    _immobilized = info.GetBoolean("Immobilized");
                    _incapacitated = info.GetBoolean("Incapacitated");
                    _mindBlocked = info.GetBoolean("MindBlocked");
                    _jammed = info.GetBoolean("Jammed");
                    _handCuffed = info.GetBoolean("HandCuffed");
                    _stealthed = info.GetBoolean("Stealthed");
                    return;
                }

                _stunned = info.GetBoolean("CharacterSpecialStates+Stunned");
                _immobilized = info.GetBoolean("CharacterSpecialStates+Immobilized");
                _incapacitated = info.GetBoolean("CharacterSpecialStates+Incapacitated");
                _mindBlocked = info.GetBoolean("CharacterSpecialStates+MindBlocked");
                _jammed = info.GetBoolean("CharacterSpecialStates+Jammed");
                _handCuffed = info.GetBoolean("CharacterSpecialStates+HandCuffed");
                _stealthed = info.GetBoolean("CharacterSpecialStates+Stealthed");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CSS:S", _stunned);
            info.AddValue("CSS:I", _immobilized);
            info.AddValue("CSS:I1", _incapacitated);
            info.AddValue("CSS:MB", _mindBlocked);
            info.AddValue("CSS:J", _jammed);
            info.AddValue("CSS:HC", _handCuffed);
            info.AddValue("CSS:S1", _stealthed);
            info.AddValue("CSS:F", _f);
            info.AddValue("CSS:P", _p);
            info.AddValue("CSS:N", _n);
            info.AddValue("CSS:T", _t);
            info.AddValue("CSS:R", _r);
        }

        public bool _stunned;

        public bool _immobilized;

        public bool _incapacitated;

        public bool _mindBlocked;

        public bool _jammed;

        public bool _handCuffed;

        public bool _stealthed;

        public bool _f;

        public bool _p;

        public bool _n;

        public bool _t;

        public bool _r;
    }
}
