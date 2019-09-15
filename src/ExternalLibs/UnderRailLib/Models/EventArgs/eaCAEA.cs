using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaCAEA")]
    public class eaCAEA : System.EventArgs
    {
        public AttackResult AttackResult { get; }
        public ARS ARS { get; }
        
        public bool Bool { get; }

        public eaCAEA(AttackResult attackResult, ARS ars, bool b = false)
        {
            AttackResult = attackResult;
            ARS = ars;
            Bool= b;
        }
    }
}
