using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEA1")]
    [Serializable]
    public class SEA1 : Aspect<LE2>, iMAIA, iINA
    {
        protected SEA1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _shieldVisibility = info.GetDouble("SEA1:SV");
                _phase = info.GetInt32("SEA1:P");
                _phaseCache = info.GetDouble("SEA1:PC");
                _shieldHitCache = info.GetDouble("SEA1:SHC");
                _shieldState = (eVSS)info.GetValue("SEA1:SS", typeof(eVSS));
                _shieldColor = (Color)info.GetValue("SEA1:SC", typeof(Color));
                _shieldHitColor = (Color)info.GetValue("SEA1:SHC1", typeof(Color));
                _shieldModel = info.GetString("SEA1:SM");
                return;
            }
            _shieldVisibility = info.GetDouble("_ShieldVisibility");
            _phase = info.GetInt32("_Phase");
            _phaseCache = info.GetDouble("_PhaseCache");
            _shieldHitCache = info.GetDouble("_ShieldHitCache");
            _shieldState = (eVSS)info.GetValue("_ShieldState", typeof(eVSS));
            _shieldColor = (Color)info.GetValue("_ShieldColor", typeof(Color));
            _shieldHitColor = (Color)info.GetValue("_ShieldHitColor", typeof(Color));
            _shieldModel = info.GetString("_ShieldModel");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SEA1:SV", _shieldVisibility);
            info.AddValue("SEA1:P", _phase);
            info.AddValue("SEA1:PC", _phaseCache);
            info.AddValue("SEA1:SHC", _shieldHitCache);
            info.AddValue("SEA1:SS", _shieldState);
            info.AddValue("SEA1:SC", _shieldColor);
            info.AddValue("SEA1:SHC1", _shieldHitColor);
            info.AddValue("SEA1:SM", _shieldModel);
        }

        private double _shieldVisibility;

        private int _phase;

        private double _phaseCache;

        private double _shieldHitCache;

        private eVSS _shieldState;

        private double f;

        private Color _shieldColor;

        private Color _shieldHitColor;

        private string _shieldModel;
    }
}
