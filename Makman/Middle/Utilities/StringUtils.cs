

namespace Makman.Middle.Utilities
{
    public static class StringUtils
    {
        public static string? GetFileExtension(string fileName)
        {
            if (fileName.Contains('.'))
            {
                return fileName.Split('.').Last();
            }
            return null;
        }

        public static string ReplaceFileName(string fullFileName, string newFileName)
        {
            var oldName = GetFileName(fullFileName);
            return fullFileName.Replace(oldName, newFileName);
        }

        public static string ReplaceFileDirectory(string fullFileName, string newFileDirectory)
        {
            var fileName = GetFileName(fullFileName);
            return newFileDirectory + "\\" + fileName;
        }

        public static string GetFileName(this string fullFileName)
        {
            return fullFileName.Split("\\").Last();
        }

        public static string AddPostfixToFileName(string fileName, string postfix)
        {
            var ext = GetFileExtension(fileName);

            if (ext == null)
                return fileName + postfix;

            var i = fileName.LastIndexOf(ext) - 1;
            return fileName.Insert(i, postfix);
        }
    }
}
