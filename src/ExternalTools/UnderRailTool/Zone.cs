using System.Collections.Generic;
using System.Linq;

namespace UnderRailTool
{
    public static class Zone
    {
        // "Z" class = cfs
        // "UEA" class = dsg
        // "SYJ" class = bwg
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<cfs>(gameFile, true);
            var result = new Dictionary<string, string>();

            foreach (ama area in model.Areas)
            {
                foreach (bko entity in area.Entities)
                {
                    string id = entity.n6().ToString(); // base get "I"
                    string name = entity.nj(); // base get "N"
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }

                    foreach (b4y aspect in entity.Aspects)
                    {
                        if (!(aspect is dsg translatableAspect))
                        {
                            continue;
                        }

                        List<cd6> jobs = translatableAspect.r();
                        foreach (string text in jobs.OfType<bwg>().Select(job => job.a()).Where(text => !string.IsNullOrEmpty(text)))
                        {
                            result.Add($"Job-{id}", text);
                        }
                    }
                }
            }

            return result;
        }

        public static cfs SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<cfs>(gameFile, true);

            foreach (ama area in model.Areas)
            {
                foreach (bko entity in area.Entities)
                {
                    string id = entity.n6().ToString();
                    if (texts.TryGetValue(id, out string name))
                    {
                        entity.q(name);
                    }

                    foreach (b4y aspect in entity.Aspects)
                    {
                        if (!(aspect is dsg translatableAspect))
                        {
                            continue;
                        }

                        List<cd6> jobs = translatableAspect.r();
                        foreach (cd6 job in jobs)
                        {
                            if (!(job is bwg translatableJob))
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
