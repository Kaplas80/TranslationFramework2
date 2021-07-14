using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Dialogs
    {
        // "DM" class = auo
        // "CE" class = dre
        // "Q" class = abi
        // "SE" class = dk0
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<auo>(gameFile, false);

            return GetSubtitles(model);
        }

        public static auo SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<auo>(gameFile, false);

            SetSubtitles(model, texts);

            return model;
        }

        private static Dictionary<string, string> GetSubtitles(auo model)
        {
            var result = new Dictionary<string, string>();

            if (model == null)
            {
                return result;
            }

            var processed = new Dictionary<string, bool>();

            var queue = new Queue<dre>();

            dre startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                dre current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case abi question:
                    {
                        Dictionary<string, string> dict = question.d(); // LocalizedTexts

                        if (dict.Count > 0 && !result.ContainsKey(question.Name))
                        {
                            string str = dict["English"];
                            result.Add(question.Name, str);
                        }

                        foreach (ccc answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case dk0 storyElement:
                    {
                        Dictionary<string, string> dict = storyElement.d();

                        if (dict.Count > 0 && !result.ContainsKey(storyElement.Name))
                        {
                            string str = dict["English"];
                            result.Add(storyElement.Name, str);
                        }

                        break;
                    }
                }

                foreach (dre step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }

            return result;
        }

        private static void SetSubtitles(auo model, Dictionary<string, string> dictionary)
        {
            var processed = new Dictionary<string, bool>();
            var queue = new Queue<dre>();
            dre startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                dre current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case abi question:
                    {
                        Dictionary<string, string> dict = question.d();

                        if (dictionary.ContainsKey(question.Name))
                        {
                            string str = dictionary[question.Name];
                            dict["English"] = str;
                        }

                        foreach (ccc answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case dk0 storyElement:
                    {
                        Dictionary<string, string> dict = storyElement.d();

                        if (dictionary.ContainsKey(storyElement.Name))
                        {
                            string str = dictionary[storyElement.Name];
                            dict["English"] = str;
                        }

                        break;
                    }
                }

                foreach (dre step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }
        }
    }
}
