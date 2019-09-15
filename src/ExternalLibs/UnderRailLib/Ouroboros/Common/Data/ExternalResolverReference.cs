using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Common.Data
{
    [EncodedTypeName("ERR")]
    [Serializable]
    public sealed class ExternalResolverReference<T> : TypedObjectReference<T>, ISerializable where T : class
    {
        private ExternalResolverReference(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _resolver = (IResolver<T>)info.GetValue("ERR:R", typeof(IResolver<T>));
                _referenceContainer = info.GetString("ERR:RC");
                return;
            }
            if (GetType() == typeof(TypedObjectReference<T>))
            {
                _resolver = (IResolver<T>)info.GetValue("_Resolver", typeof(IResolver<T>));
                _referenceContainer = info.GetString("_ReferenceContainer");
                return;
            }
            _resolver = (IResolver<T>)info.GetValue("ExternalResolverReference`1+_Resolver", typeof(IResolver<T>));
            _referenceContainer = info.GetString("ExternalResolverReference`1+_ReferenceContainer");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ERR:R", _resolver);
            info.AddValue("ERR:RC", _referenceContainer);
        }

        private IResolver<T> _resolver;

        private string _referenceContainer;
    }
}
