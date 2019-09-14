using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Vehicles
{
    [EncodedTypeName("eVPT")]
    [Serializable]
    public enum VehiclePartType
    {
        Frame,
        Engine,
        Battery,
        JetskiSuspension,
        Harp1Rocket = 100
    }
}
