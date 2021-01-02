using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Dialogs
    {
        // "DM" class = aqq
        // "CE" class = dd2
        // "Q" class = yz
        // "SE" class = c74
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<aqq>(gameFile, false);

            return GetSubtitles(model);
        }

        public static aqq SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<aqq>(gameFile, false);

            SetSubtitles(model, texts);

            return model;
        }

        private static Dictionary<string, string> GetSubtitles(aqq model)
        {
            var result = new Dictionary<string, string>();

            if (model == null)
            {
                return result;
            }

            var processed = new Dictionary<string, bool>();

            var queue = new Queue<dd2>();

            dd2 startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                dd2 current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case yz question:
                    {
                        Dictionary<string, string> dict = question.d(); // LocalizedTexts

                        if (dict.Count > 0 && !result.ContainsKey(question.Name))
                        {
                            string str = dict["English"];
                            result.Add(question.Name, str);
                        }

                        foreach (dpy answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case c74 storyElement:
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

                foreach (dd2 step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }

            return result;
        }

        private static void SetSubtitles(aqq model, Dictionary<string, string> dictionary)
        {
            var processed = new Dictionary<string, bool>();
            var queue = new Queue<dd2>();
            dd2 startElement = model.b();

            queue.Enqueue(startElement);

            while (queue.Count > 0)
            {
                dd2 current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case yz question:
                    {
                        Dictionary<string, string> dict = question.d();

                        if (dictionary.ContainsKey(question.Name))
                        {
                            string str = dictionary[question.Name];
                            dict["English"] = str;
                        }

                        foreach (dpy answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case c74 storyElement:
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

                foreach (dd2 step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }
        }
    }
}
