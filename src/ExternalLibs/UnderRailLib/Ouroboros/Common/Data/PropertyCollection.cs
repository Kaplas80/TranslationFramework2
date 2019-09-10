using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Common.Data
{
    [EncodedTypeName("PC")]
	[Serializable]
	public sealed class PropertyCollection : ISerializable
	{
		private PropertyCollection(SerializationInfo info, StreamingContext ctx)
		{
			if (DataModelVersion.MajorVersion != 0)
			{
				SerializationHelper.ReadList("PC:P", ref _properties, info);
				return;
			}
			if (GetType() == typeof(PropertyCollection))
			{
				_properties = (List<Property>)info.GetValue("_Properties", typeof(List<Property>));
				return;
			}
			_properties = (List<Property>)info.GetValue("PropertyCollection+_Properties", typeof(List<Property>));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext ctx)
		{
            SerializationHelper.WriteList("PC:P", _properties, info);
		}

		private List<Property> _properties = new List<Property>();

        public PropertyCollection()
        {
        }
	}
}
