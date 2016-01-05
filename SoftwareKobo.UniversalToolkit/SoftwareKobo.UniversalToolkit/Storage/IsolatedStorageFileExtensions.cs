using System.IO.IsolatedStorage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    /// <summary>
    /// IsolatedStorageFile 扩展类。
    /// </summary>
    public static class IsolatedStorageFileExtensions
    {
        /// <summary>
        /// 递归删除独立存储目录的某个文件夹及其子级内容。
        /// </summary>
        /// <param name="directory">需要删除的目录文件夹路径。</param>
        public static void DeleteDirectoryRecursive(string directory)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(directory))
                {
                    var files = isf.GetFileNames(directory + @"/*");
                    foreach (var file in files)
                    {
                        isf.DeleteFile(directory + @"/" + file);
                    }

                    var subDirectories = isf.GetDirectoryNames(directory + @"/");
                    foreach (var subDirectory in subDirectories)
                    {
                        DeleteDirectoryRecursive(directory + @"/" + subDirectory);
                    }

                    isf.DeleteDirectory(directory);
                }
            }
        }

        public static long GetDirectorySize(string directory)
        {
            long directorySize;
            GetDirectorySize(directory, out directorySize);
            return directorySize;
        }

        private static void GetDirectorySize(string directory, out long directorySize)
        {
            directorySize = 0;
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(directory))
                {
                    var files = isf.GetFileNames(directory + @"/*");
                    foreach (var file in files)
                    {
                        var fileSize = GetFileSize(directory + @"/" + file);
                        directorySize += fileSize;
                    }

                    var subDirectories = isf.GetDirectoryNames(directory + @"/");
                    foreach (var subDirectory in subDirectories)
                    {
                        long subDirectorySize;
                        GetDirectorySize(directory + @"/" + subDirectory, out subDirectorySize);
                        directorySize += directorySize;
                    }
                }
            }
        }

        private static long GetFileSize(string filePath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = isf.OpenFile(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return fileStream.Length;
                }
            }
        }
    }
}