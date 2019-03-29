using System;
using System.Text.RegularExpressions;

namespace YakuzaGame.Files.Msg
{
    public class Subtitle : TF.Core.TranslationEntities.Subtitle
    {
        public MsgProperties Properties { get; set; }

        public MsgProperties TranslationProperties
        {
            get
            {
                var newProperties = new MsgProperties();

                var translationProperties = ExtractProperties(Translation);
                newProperties.AddRange(translationProperties);


                var i = 0;
                foreach (var property in Properties)
                {
                    if (property.GetType().Name == nameof(MsgProperty))
                    {
                        var copy = new MsgProperty(property.ToByteArray());
                        if (property.IsEndProperty)
                        {
                            copy.Position = (short) CleanTranslation().Length;
                        }

                        copy.Order = i;
                        
                        newProperties.Add(copy);
                    }

                    i++;
                }

                return newProperties;
            }
        }

        public Subtitle() : base()
        {

        }

        public Subtitle(TF.Core.TranslationEntities.Subtitle s) : base()
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
        }

        private static string Clean(string text, bool removeLineBreaks, bool removeTags)
        {
            var lineBreakPattern = @"(\\r\\n)|(\\n)";
            var colorPattern = @"(<Color[^>]*>)";
            var tagPattern = @"(<[^>]*>)";

            var temp = text;
            if (removeLineBreaks)
            {
                temp = Regex.Replace(temp, lineBreakPattern, "#");
            }

            if (removeTags)
            {
                temp = Regex.Replace(temp, colorPattern, string.Empty);
                temp = Regex.Replace(temp, tagPattern, "#");
            }

            return temp;
        }

        public string CleanText()
        {
            return Clean(Text, true, true);
        }

        public string CleanTranslation()
        {
            return Clean(Translation, true, true);
        }

        private static MsgProperties ExtractProperties(string text)
        {
            var result = new MsgProperties();

            var cleanText = Clean(text, true, true);
            var textWithTags = Clean(text, true, false);

            var shortPauses = Regex.Matches(cleanText, @"(,\s)|(、)");

            foreach (Match pause in shortPauses)
            {
                var property = new PauseMsgProperty {Duration = 10, Position = (short) (pause.Index + 1), Order = -1};
                result.Add(property);
            }

            var longPauses = Regex.Matches(cleanText, @"([\?|？][\s|#])|([\!|！][\s|#])|(\.\s)|(。[\s]*)|(…[^…）？！])");

            foreach (Match pause in longPauses)
            {
                var property = new PauseMsgProperty { Duration = 20, Position = (short)(pause.Index + 1), Order = -1 };
                result.Add(property);
            }

            var openParenthesis = Regex.Matches(cleanText, @"([\(|（])");

            foreach (Match parenthesis in openParenthesis)
            {
                var property = new ParenthesisStartMsgProperty { Position = (short)(parenthesis.Index), Order = -1 };
                result.Add(property);
            }

            var closeParenthesis = Regex.Matches(cleanText, @"([\)|）])");

            foreach (Match parenthesis in closeParenthesis)
            {
                var property = new ParenthesisEndMsgProperty { Position = (short)(parenthesis.Index + 1), Order = -1 };
                result.Add(property);
            }

            var tagMatch = Regex.Match(textWithTags, @"(<Sign:(?<Code>\d+)>)|(<Color:(?<Code>(Default|\d+|\d+,\d+,\d+,\d+))>)");
            while (tagMatch.Success)
            {
                if (tagMatch.Value.Contains("Sign"))
                {
                    var property = new SignMsgProperty()
                    {
                        Position = (short) tagMatch.Index,
                        Code = Convert.ToInt16(tagMatch.Groups["Code"].Value),
                        TagLength = (short) tagMatch.Length,
                        Order = -1
                    };
                    result.Add(property);

                    textWithTags = string.Concat(textWithTags.Substring(0, tagMatch.Index), "#",
                        textWithTags.Substring(tagMatch.Index + tagMatch.Length));
                }

                if (tagMatch.Value.Contains("Color"))
                {
                    var color = tagMatch.Groups["Code"].Value;

                    if (color == "Default")
                    {
                        var property = new ColorEndMsgProperty()
                        {
                            Position = (short)(tagMatch.Index),
                            Order = -1
                        };
                        result.Add(property);
                    }
                    else
                    {
                        if (color.Contains(","))
                        {
                            var values = color.Split(',');
                            var property = new ColorStartMsgProperty
                            {
                                Position = (short)(tagMatch.Index),
                                Color = Convert.ToInt16(values[3]),
                                Alpha = Convert.ToByte(values[3]),
                                R = Convert.ToByte(values[0]),
                                G = Convert.ToByte(values[1]),
                                B = Convert.ToByte(values[2]),
                                TagLength = (short)tagMatch.Length,
                                Order = -1
                            };
                            result.Add(property);
                        }
                        else
                        {
                            var property = new ColorStartMsgProperty
                            {
                                Position = (short)(tagMatch.Index),
                                Color = Convert.ToInt16(color),
                                Alpha = 0,
                                R = 0,
                                G = 0,
                                B = 0,
                                TagLength = (short)tagMatch.Length,
                                Order = -1
                            };
                            result.Add(property);
                        }
                    }

                    textWithTags = string.Concat(textWithTags.Substring(0, tagMatch.Index),  
                        textWithTags.Substring(tagMatch.Index + tagMatch.Length));
                }
                
                tagMatch = Regex.Match(textWithTags, @"(<Sign:(?<Code>\d+)>)|(<Color:(?<Code>(Default|\d+|\d+,\d+,\d+,\d+))>)");
            }

            return result;
        }
    }
}
