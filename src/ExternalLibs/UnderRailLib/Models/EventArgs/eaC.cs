using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaC")]
    public sealed class eaC : eaCAEA
    {
        public List<Damage> ListDamages1 { get; }
        public List<Damage> ListDamages2 { get; }

        public eaC(AttackResult attackResult, ARS ars, List<Damage> listDamages1, List<Damage> listDamages2) : base(attackResult, ars, false)
        {
            ListDamages1 = listDamages1;
            ListDamages2 = listDamages2;
        }
    }
}
