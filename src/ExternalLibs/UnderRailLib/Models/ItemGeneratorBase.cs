using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IGB")]
    [Serializable]
    public abstract class ItemGeneratorBase : anu, ISerializable
    {
        protected ItemGeneratorBase(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _minimalLevel = info.GetInt32("IGB:ML");
                _maximalLevel = info.GetInt32("IGB:ML1");
                if (DataModelVersion.MinorVersion >= 406)
                {
                    _b = (info.GetValue("IGB:B", typeof(int?)) as int?);
                    _d = (info.GetValue("IGB:D", typeof(int?)) as int?);
                    _o = (info.GetValue("IGB:O", typeof(bool)) as bool?);
                }

                if (DataModelVersion.MinorVersion >= 427)
                {
                    _i = (info.GetValue("IGB:I", typeof(bool?)) as bool?);
                }
            }
            else
            {
                if (GetType() == typeof(ItemGeneratorBase))
                {
                    _minimalLevel = info.GetInt32("_MinimalLevel");
                    _maximalLevel = info.GetInt32("_MaximalLevel");
                    return;
                }

                _minimalLevel = info.GetInt32("ItemGeneratorBase+_MinimalLevel");
                _maximalLevel = info.GetInt32("ItemGeneratorBase+_MaximalLevel");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IGB:ML", _minimalLevel);
            info.AddValue("IGB:ML1", _maximalLevel);
            info.AddValue("IGB:B", _b);
            info.AddValue("IGB:D", _d);
            info.AddValue("IGB:O", _o);
            info.AddValue("IGB:I", _i);
        }

        private int _minimalLevel = 1;

        private int _maximalLevel = 1;

        private int? _b;

        private int? _d;

        private bool? _o;

        private bool? _i;
    }
}
