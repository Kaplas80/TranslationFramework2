using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Dialogs
    {
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<ap0>(gameFile, false);

            return GetSubtitles(model);
        }

        public static ap0 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<ap0>(gameFile, false);

            SetSubtitles(model, texts);

            return model;
        }

        private static Dictionary<string, string> GetSubtitles(ap0 model)
        {
            var result = new Dictionary<string, string>();

            if (model == null)
            {
                return result;
            }

            var processed = new Dictionary<string, bool>();

            var queue = new Queue<dcr>();

            var startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case cu7 question:
                    {
                        var dict = question.d(); // LocalizedTexts

                        if (dict.Count > 0 && !result.ContainsKey(question.Name))
                        {
                            var str = dict["English"];
                            result.Add(question.Name, str);
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case c6z storyElement:
                    {
                        var dict = storyElement.d();

                        if (dict.Count > 0 && !result.ContainsKey(storyElement.Name))
                        {
                            var str = dict["English"];
                            result.Add(storyElement.Name, str);
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }

            return result;
        }

        private static void SetSubtitles(ap0 model, Dictionary<string, string> dictionary)
        {
            var processed = new Dictionary<string, bool>();
            var queue = new Queue<dcr>();
            var startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case cu7 question:
                    {
                        var dict = question.d();

                        if (dictionary.ContainsKey(question.Name))
                        {
                            var str = dictionary[question.Name];
                            dict["English"] = str;
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case c6z storyElement:
                    {
                        var dict = storyElement.d();

                        if (dictionary.ContainsKey(storyElement.Name))
                        {
                            var str = dictionary[storyElement.Name];
                            dict["English"] = str;
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }
        }
    }
}
