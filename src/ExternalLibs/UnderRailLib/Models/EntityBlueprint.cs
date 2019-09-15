using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Common.Data;
using UnderRailLib.Ouroboros.Playfield.Design;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EB")]
    [Serializable]
    public sealed class EntityBlueprint : ISerializable
    {
        private EntityBlueprint(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("EB:N");
                _generalCategory = (GeneralEntityCategory)info.GetValue("EB:GC", typeof(GeneralEntityCategory));
                _subCategory = info.GetString("EB:SC");
                _template = info.GetString("EB:T");
                _path = info.GetString("EB:P");
                _parameters = (PropertyCollection)info.GetValue("EB:P1", typeof(PropertyCollection));
                return;
            }
            if (GetType() == typeof(EntityBlueprint))
            {
                _name = info.GetString("_Name");
                _generalCategory = (GeneralEntityCategory)info.GetValue("_GeneralCategory", typeof(GeneralEntityCategory));
                _subCategory = info.GetString("_SubCategory");
                _template = info.GetString("_Template");
                _path = info.GetString("_Path");
                _parameters = (PropertyCollection)info.GetValue("_Parameters", typeof(PropertyCollection));
                return;
            }
            _name = info.GetString("EntityBlueprint+_Name");
            _generalCategory = (GeneralEntityCategory)info.GetValue("EntityBlueprint+_GeneralCategory", typeof(GeneralEntityCategory));
            _subCategory = info.GetString("EntityBlueprint+_SubCategory");
            _template = info.GetString("EntityBlueprint+_Template");
            _path = info.GetString("EntityBlueprint+_Path");
            _parameters = (PropertyCollection)info.GetValue("EntityBlueprint+_Parameters", typeof(PropertyCollection));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("EB:N", _name);
            info.AddValue("EB:GC", _generalCategory);
            info.AddValue("EB:SC", _subCategory);
            info.AddValue("EB:T", _template);
            info.AddValue("EB:P", _path);
            info.AddValue("EB:P1", _parameters);
        }

        private string _name;

        private GeneralEntityCategory _generalCategory;
        
        private string _subCategory;

        private string _template;

        private string _path;

        private PropertyCollection _parameters = new PropertyCollection();
    }
}
