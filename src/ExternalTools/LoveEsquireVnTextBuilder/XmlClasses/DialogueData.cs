namespace LoveEsquireVnTextBuilder.XmlClasses
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("DialogueData")]
    [Serializable]
    public class DialogueData
    {
        public DialogueData()
        {
        }

        [XmlArray("DialogueSets")]
        [XmlArrayItem("DialogueSet")]
        public List<DialogueSet> dialogueGroups = new List<DialogueSet>();
    }

    public class DialogueSet
    {
        public DialogueSet()
        {
        }

        [XmlAttribute("variable")]
        public string variable;

        [XmlAttribute("triggerPointID")]
        public string triggerPointID;

        [XmlArray("Dialogues")]
        [XmlArrayItem("Dialogue")]
        public List<Dialogue> dialogues = new List<Dialogue>();
    }
}
