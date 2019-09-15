using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Ouroboros.Common.Aspects
{
    [EncodedTypeName("A1")]
    [Serializable]
    public abstract class Aspect<TContainer> : A, ISerializable where TContainer : class, iIAC
    {
        protected Aspect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
