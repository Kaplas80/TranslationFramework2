using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaIEA")]
    [Serializable]
    public sealed class ItemEventArgs : System.EventArgs
    {
        public II Item { get; }

        public ItemEventArgs(II item)
        {
            Item = item;
        }
    }
}
