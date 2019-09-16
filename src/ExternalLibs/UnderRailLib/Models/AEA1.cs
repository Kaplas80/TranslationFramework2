using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.Ouroboros.Playfield.References;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AEA1")]
    [Serializable]
    public sealed class AEA1 : Aspect<E4>, iINA
    {
        private AEA1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadDictionary("AEA1:A", ref _a, info);
                if (DataModelVersion.MinorVersion >= 270)
                {
                    _pa = (info.GetValue("AEA1:PA", typeof(PCAC)) as PCAC);
                    return;
                }
                var keyValuePair = (KeyValuePair<ActionReference<iPRCHA>, eAEXT>?)info.GetValue("AEA1:PA", typeof(KeyValuePair<ActionReference<iPRCHA>, eAEXT>?));
                if (keyValuePair?.Key != null)
                {
                    _pa = new PCAC(keyValuePair.Value.Key, keyValuePair.Value.Value);
                }
            }
            else
            {
                _a = new Dictionary<ActionReference<iTLACTI>, eAEXT>();
                SerializationHelper.ReadDictionary("_Actions", ref _a, info);
                var keyValuePair2 = info.GetValue("_PrimaryAction", typeof(KeyValuePair<ActionReference<iPRCHA>, eAEXT>?)) as KeyValuePair<ActionReference<iPRCHA>, eAEXT>?;
                if (keyValuePair2?.Key != null)
                {
                    _pa = new PCAC(keyValuePair2.Value.Key, keyValuePair2.Value.Value);
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteDictionary("AEA1:A", _a, info);
            info.AddValue("AEA1:PA", _pa);
        }

        private Dictionary<ActionReference<iTLACTI>, eAEXT> _a = new Dictionary<ActionReference<iTLACTI>, eAEXT>();

        private PCAC _pa;
    }
}
