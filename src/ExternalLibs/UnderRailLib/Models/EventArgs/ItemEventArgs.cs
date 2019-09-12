using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaIEA")]
    [Serializable]
    public sealed class ItemEventArgs : System.EventArgs
    {
        public ItemInstance Item { get; }

        public ItemEventArgs(ItemInstance item)
        {
            Item = item;
        }
    }
}
