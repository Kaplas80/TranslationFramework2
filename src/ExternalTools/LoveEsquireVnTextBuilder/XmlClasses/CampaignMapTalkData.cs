namespace LoveEsquireVnTextBuilder.XmlClasses
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("CampaignMapTalkData")]
    public class CampaignMapTalkData
    {
        public CampaignMapTalkData()
        {
        }

        [XmlArray("CampaignMapTalkGroups")]
        [XmlArrayItem("CampaignMapTalkGroup")]
        public List<CampaignMapTalkGroup> talkGroups = new List<CampaignMapTalkGroup>();
    }

    [Serializable]
    public class CampaignMapTalkGroup
    {
        public CampaignMapTalkGroup()
        {
        }

        [XmlAttribute("id")]
        public string id;

        [XmlArray("CampaignMapTalks")]
        [XmlArrayItem("CampaignMapTalk")]
        public List<CampaignMapTalk> campaignMapTalks = new List<CampaignMapTalk>();
    }

    [Serializable]
    public class CampaignMapTalk
    {
        public CampaignMapTalk()
        {
        }

        [XmlAttribute("id")]
        public string id;

        [XmlAttribute("text")]
        public string text;

        [XmlAttribute("vaFile")]
        public string vaFile;

        [XmlAttribute("characterExpression")]
        public string characterExpression;
    }
}
