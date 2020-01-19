namespace LoveEsquireVnTextBuilder.XmlClasses
{
    using System.Xml.Serialization;

    public class Dialogue
    {
        public Dialogue()
        {
        }

        [XmlAttribute("speaker")]
        public string speaker;

        [XmlAttribute("line")]
        public string line;

        [XmlAttribute("va")]
        public string va;

        [XmlAttribute("condition")]
        public string condition;
    }
}
