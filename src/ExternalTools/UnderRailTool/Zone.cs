using System.Collections.Generic;
using System.Linq;

namespace UnderRailTool
{
    public static class Zone
    {
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<ces>(gameFile, true);

            var result = new Dictionary<string, string>();

            foreach (var area in model.Areas)
            {
                foreach (var entity in area.Entities)
                {
                    var id = entity.n2().ToString();
                    var name = entity.nb();
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }

                    foreach (var aspect in entity.Aspects)
                    {
                        if (!(aspect is dqx dqxAspect))
                        {
                            continue;
                        }

                        var jobs = dqxAspect.r();
                        foreach (var text in jobs.OfType<bvz>().Select(job => job.a()).Where(text => !string.IsNullOrEmpty(text)))
                        {
                            result.Add($"Job-{id}", text);
                        }
                    }
                }
            }

            return result;
        }

        public static ces SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<ces>(gameFile, true);

            foreach (var area in model.Areas)
            {
                foreach (var entity in area.Entities)
                {
                    var id = entity.n2().ToString();
                    if (texts.TryGetValue(id, out var name))
                    {
                        entity.q(name);
                    }

                    foreach (var aspect in entity.Aspects)
                    {
                        if (!(aspect is dqx dqxAspect))
                        {
                            continue;
                        }

                        var jobs = dqxAspect.r();
                        foreach (var job in jobs)
                        {
                            if (!(job is bvz bvzJob))
                            {
                                continue;
                            }

                            if (texts.TryGetValue($"Job-{id}", out var text))
                            {
                                bvzJob.a(text);
                            }
                        }
                    }
                }
            }

            return model;
        }
    }
}
