namespace LoveEsquireVnTextBuilder.XmlClasses
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("BanterData")]
    [Serializable]
    public class BanterData
    {
        public BanterData()
        {
        }

        [XmlArray("BanterGroups")]
        [XmlArrayItem("BanterGroup")]
        public List<BanterGroup> banterGroups;
    }

    public class BanterGroup
    {
        public BanterGroup()
        {
        }

        [XmlAttribute("banterID")]
        public string banterID;

        [XmlAttribute("rate")]
        public float rate;

        [XmlAttribute("type")]
        public string type;

        [XmlArray("StarterDialogues")]
        [XmlArrayItem("Dialogue")]
        public List<Dialogue> starterDialogues;

        [XmlArray("ResponseDialogues")]
        [XmlArrayItem("Dialogue")]
        public List<Dialogue> responseDialogues;
    }
}
