using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaCH")]
    public class eaCH : System.EventArgs
    {
        public int Int { get; }

        public CharacterInfo CharacterInfo { get; }

        public object Object { get; }

        public eaCH(int i, CharacterInfo characterInfo = null, object obj = null)
        {
            Int = i;
            CharacterInfo = characterInfo;
            Object = obj;
        }
    }

}
