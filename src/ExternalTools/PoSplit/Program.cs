using System;

namespace PoSplit
{
    using System.IO;
    using Yarhl.FileFormat;
    using Yarhl.IO;
    using Yarhl.Media.Text;

    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = @"C:\Users\Kaplas\Desktop\Po Split\Disco Elysium_SELFNAME.po";
            string outputFolder = @"C:\Users\Kaplas\Desktop\Po Split";

            string fileName = Path.GetFileNameWithoutExtension(inputFile);

            using (DataStream dataStream = DataStreamFactory.FromFile(inputFile, FileOpenMode.Read))
            {
                var binary = new BinaryFormat(dataStream);
                var po2binary = new Yarhl.Media.Text.Po2Binary();

                Po inputPo = (Po)ConvertFormat.With<Po2Binary>(binary);
                var outputPo = new Po()
                {
                    Header = inputPo.Header
                };

                var ctx = inputPo.Entries[0].Context;
                var tags = ctx.Split('_');
                var previousTag0 = tags[0];
                var previousTag1 = tags[1];

                int index = 1;
                foreach (PoEntry entry in inputPo.Entries)
                {
                    ctx = entry.Context;
                    tags = ctx.Split('_');
                    var tag0 = tags[0];
                    var tag1 = tags[1];
                    if (tag0 != previousTag0)
                    {
                        var outputBinary = po2binary.Convert(outputPo);
                        var path = Path.Combine(outputFolder, $"{fileName}.{previousTag0}.po");
                        outputBinary.Stream.WriteTo(path);

                        outputPo = new Po()
                        {
                            Header = inputPo.Header
                        };

                    }
                    else if (tag0 == "Conversation" && tag1 != previousTag1 && outputPo.Entries.Count >= 2500)
                    {
                        var outputBinary = po2binary.Convert(outputPo);
                        var path = Path.Combine(outputFolder, $"{fileName}.{previousTag0}_{index}.po");
                        outputBinary.Stream.WriteTo(path);

                        outputPo = new Po()
                        {
                            Header = inputPo.Header
                        };

                        index++;
                    }

                    outputPo.Add(entry);

                    previousTag0 = tag0;
                    previousTag1 = tag1;
                }
                
                var outputBinary1 = po2binary.Convert(outputPo);
                var path1 = Path.Combine(outputFolder, $"{fileName}.{previousTag0}_{index}.po");
                outputBinary1.Stream.WriteTo(path1);

            }
        }
    }
}
