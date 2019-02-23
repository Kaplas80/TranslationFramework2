using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TF.Core.Helpers
{
    public static class PathHelper
    {
        [DllImport("shlwapi.dll", SetLastError = true)]
        private static extern int PathRelativePathTo(StringBuilder pszPath, string pszFrom, int dwAttrFrom, string pszTo, int dwAttrTo);

        private const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
        private const int FILE_ATTRIBUTE_NORMAL = 0x80;

        public static string GetRelativePath(string fromPath, string toPath)
        {
            var fromAttr = GetPathAttribute(fromPath);
            var toAttr = GetPathAttribute(toPath);

            var path = new StringBuilder(260); // MAX_PATH
            if (PathRelativePathTo(path, fromPath, fromAttr, toPath, toAttr) == 0)
            {
                throw new ArgumentException("Paths must have a common prefix");
            }

            return path.ToString();
        }

        private static int GetPathAttribute(string path)
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                return FILE_ATTRIBUTE_DIRECTORY;
            }

            var fi = new FileInfo(path);
            if (fi.Exists)
            {
                return FILE_ATTRIBUTE_NORMAL;
            }

            throw new FileNotFoundException();
        }

        public static void CloneDirectory(string source, string dest)
        {
            foreach (var directory in Directory.GetDirectories(source))
            {
                var dirName = Path.GetFileName(directory);
                Directory.CreateDirectory(Path.Combine(dest, dirName));
                CloneDirectory(directory, Path.Combine(dest, dirName));
            }

            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
            }
        }

        public static void DeleteDirectory(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                var dirName = Path.GetFileName(directory);
                Directory.Delete(dirName, true);
            }

            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
        }
    }
}
