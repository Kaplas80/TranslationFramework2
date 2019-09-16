using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MEA")]
    [Serializable]
    public sealed class MobileEntityAspect : Aspect<E4>, ISerializable
    {
        private MobileEntityAspect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _currentSpeed = info.GetSingle("MEA:CS");
                _currentMomentum = (Vector3)info.GetValue("MEA:CM", typeof(Vector3));
                _maxSpeed = info.GetSingle("MEA:MS");
                _startSpeed = info.GetSingle("MEA:SS");
                _movementHandler = (IMovementHandler)info.GetValue("MEA:MH", typeof(IMovementHandler));
                _mobileEntity = (IMobile)info.GetValue("MEA:ME", typeof(IMobile));
                return;
            }
            _currentSpeed = info.GetSingle("_CurrentSpeed");
            _currentMomentum = (Vector3)info.GetValue("_CurrentMomentum", typeof(Vector3));
            _maxSpeed = info.GetSingle("_MaxSpeed");
            _startSpeed = info.GetSingle("_StartSpeed");
            _movementHandler = (info.GetValue("_MovementHandler", typeof(IMovementHandler)) as IMovementHandler);
            _mobileEntity = (info.GetValue("_MobileEntity", typeof(IMobile)) as IMobile);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MEA:CS", _currentSpeed);
            info.AddValue("MEA:CM", _currentMomentum);
            info.AddValue("MEA:MS", _maxSpeed);
            info.AddValue("MEA:SS", _startSpeed);
            info.AddValue("MEA:MH", _movementHandler);
            info.AddValue("MEA:ME", _mobileEntity);
        }

        private float _currentSpeed;

        private Vector3 _currentMomentum;

        private float _maxSpeed;

        private float _startSpeed;

        private IMovementHandler _movementHandler;

        private IMobile _mobileEntity;
    }
}
