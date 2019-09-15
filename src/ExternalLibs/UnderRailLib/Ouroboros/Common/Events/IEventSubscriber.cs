using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Common.Events
{
    [EncodedTypeName("iEVt")]
    public interface IEventSubscriber<T> where T : EventArgs
    {
    }
}
