using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("I")]
    [Serializable]
    public abstract class Item : ISerializable
    {
        protected Item(SerializationInfo info, StreamingContext ctx)
        {
            _c = info.GetString("I:C");
            _n = info.GetString("I:N");
            _q = (eIQ) info.GetValue("I:Q", typeof(eIQ));
            _d = info.GetString("I:D");
            _vm = info.GetString("I:VM");
            _ms = info.GetInt32("I:MS");
            _c2 = info.GetBoolean("I:C2");
            _cp = (PropertyCollection) info.GetValue("I:CP", typeof(PropertyCollection));
            if (_cp == null)
            {
                _cp = new PropertyCollection();
            }

            if (DataModelVersion.MinorVersion >= 6)
            {
                _cv = (info.GetValue("I:CV", typeof(double?)) as double?);
                _ivf = (info.GetValue("I:IVF", typeof(iIVF)) as iIVF);
            }
            else
            {
                _cv = info.GetDouble("I:CV");
            }

            if (DataModelVersion.MinorVersion >= 45)
            {
                _l = info.GetInt32("I:L");
            }
            else
            {
                _l = 1;
            }

            if (DataModelVersion.MinorVersion >= 55)
            {
                SerializationHelper.ReadList("I:CS", ref _cs, info);
            }
            else
            {
                _cs = new List<CapabilityReference>();
                var capabilityReference = (CapabilityReference) info.GetValue("I:C1", typeof(CapabilityReference));
                if (capabilityReference != null)
                {
                    _cs.Add(capabilityReference);
                }
            }

            if (DataModelVersion.MinorVersion >= 145)
            {
                _i = info.GetBoolean("I:I");
                _r = (ItemRepairCategory) info.GetValue("I:R", typeof(ItemRepairCategory));
            }
            else
            {
                _i = true;
            }

            if (DataModelVersion.MinorVersion >= 158)
            {
                _s = (ItemStealingCategory) info.GetValue("I:S", typeof(ItemStealingCategory));
            }

            if (DataModelVersion.MinorVersion >= 191)
            {
                _t = (eTC) info.GetValue("I:T", typeof(eTC));
            }
            else
            {
                _t = eTC.b;
            }

            if (DataModelVersion.MinorVersion >= 192)
            {
                _w = info.GetDouble("I:W");
            }
            else
            {
                _w = 0.0;
            }

            if (DataModelVersion.MinorVersion >= 198)
            {
                _combatReady = info.GetBoolean("I:CR");
            }
            else
            {
                _combatReady = true;
            }

            if (DataModelVersion.MinorVersion >= 374)
            {
                _mb = (info.GetValue("I:MB", typeof(int?)) as int?);
            }

            if (DataModelVersion.MinorVersion >= 509)
            {
                _ur = info.GetBoolean("I:UR");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("I:C", _c);
            info.AddValue("I:N", _n);
            info.AddValue("I:Q", _q);

            info.AddValue("I:L", _l);

            info.AddValue("I:D", _d);
            info.AddValue("I:VM", _vm);
            info.AddValue("I:MS", _ms);
            info.AddValue("I:C2", _c2);
            info.AddValue("I:CP", _cp);
            if (DataModelVersion.MinorVersion >= 6)
            {
                info.AddValue("I:CV", _cv);
                info.AddValue("I:IVF", _ivf);
            }
            else
            {
                info.AddValue("I:CV", _cv);
            }

            if (DataModelVersion.MinorVersion >= 55)
            {
                SerializationHelper.WriteList("I:CS", _cs, info);
            }
            else
            {
                info.AddValue("I:C1", _cs[0]);
            }

            if (DataModelVersion.MinorVersion >= 145)
            {
                info.AddValue("I:I", _i);
                info.AddValue("I:R", _r);
            }

            if (DataModelVersion.MinorVersion >= 158)
            {
                info.AddValue("I:S", _s);
            }

            if (DataModelVersion.MinorVersion >= 191)
            {
                info.AddValue("I:T", _t);
            }

            if (DataModelVersion.MinorVersion >= 192)
            {
                info.AddValue("I:W", _w);
            }

            if (DataModelVersion.MinorVersion >= 198)
            {
                info.AddValue("I:CR", _combatReady);
            }

            if (DataModelVersion.MinorVersion >= 374)
            {
                info.AddValue("I:MB", _mb);
            }

            if (DataModelVersion.MinorVersion >= 509)
            {
                info.AddValue("I:UR", _ur);
            }
        }

        public const string a = "item";

        private string _c;

        private string _n = "Unknown";

        private eIQ _q = eIQ.b;

        private int _l;

        private string _d;

        private string _vm = "Misc.0";

        private List<CapabilityReference> _cs = new List<CapabilityReference>();

        private int _ms = 1;

        private bool _c2;

        private double? _cv;

        private PropertyCollection _cp = new PropertyCollection();

        private iIVF _ivf;

        private bool _i = true;

        private ItemRepairCategory _r;

        private ItemStealingCategory _s;

        private eTC _t;

        private double _w;

        private bool _combatReady = true;

        private int? _mb;

        private bool _ur;
    }
}
