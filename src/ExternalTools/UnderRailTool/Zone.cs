using System.Collections.Generic;
using System.Linq;

namespace UnderRailTool
{
    public static class Zone
    {
        // "Z" class = cp0
        // "UEA" class = b5v
        // "SYJ" class = b6e
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<cp0>(gameFile, true);
            var result = new Dictionary<string, string>();

            foreach (apt area in model.Areas)
            {
                foreach (br0 entity in area.Entities)
                {
                    string id = entity.o6().ToString(); // base get "I"
                    string name = entity.w5(); // base get "N"
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }

                    foreach (cd6 aspect in entity.Aspects)
                    {
                        if (!(aspect is b5v translatableAspect))
                        {
                            continue;
                        }

                        List<cn6> jobs = translatableAspect.s();
                        foreach (string text in jobs.OfType<b6e>().Select(job => job.a()).Where(text => !string.IsNullOrEmpty(text)))
                        {
                            result.Add($"Job-{id}", text);
                        }
                    }
                }
            }

            return result;
        }

        public static cp0 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<cp0>(gameFile, true);

            foreach (apt area in model.Areas)
            {
                foreach (br0 entity in area.Entities)
                {
                    string id = entity.o6().ToString();
                    if (texts.TryGetValue(id, out string name))
                    {
                        entity.q(name);
                    }

                    foreach (cd6 aspect in entity.Aspects)
                    {
                        if (!(aspect is b5v translatableAspect))
                        {
                            continue;
                        }

                        List<cn6> jobs = translatableAspect.s();
                        foreach (cn6 job in jobs)
                        {
                            if (!(job is b6e translatableJob))
                            {
                                continue;
                            }

                            if (texts.TryGetValue($"Job-{id}", out string text))
                            {
                                translatableJob.a(text);
                            }
                        }
                    }
                }
            }

            return model;
        }
    }
}
