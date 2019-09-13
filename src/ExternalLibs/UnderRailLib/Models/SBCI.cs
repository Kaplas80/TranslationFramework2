using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SBCI")]
    [Serializable]
    public class SBCI : ComponentItem
    {
        protected SBCI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            SerializationHelper.ReadList("SBCI:S", ref _s, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("SBCI:S", _s, info);
        }

        private List<SFY> _s = new List<SFY>();
    }
}
