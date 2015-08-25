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
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(directory))
                {
                    string[] files = isf.GetFileNames(directory + @"/*");
                    foreach (string file in files)
                    {
                        isf.DeleteFile(directory + @"/" + file);
                    }

                    string[] subDirectories = isf.GetDirectoryNames(directory + @"/");
                    foreach (string subDirectory in subDirectories)
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
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(directory))
                {
                    string[] files = isf.GetFileNames(directory + @"/*");
                    foreach (string file in files)
                    {
                        long fileSize = GetFileSize(directory + @"/" + file);
                        directorySize += fileSize;
                    }

                    string[] subDirectories = isf.GetDirectoryNames(directory + @"/");
                    foreach (string subDirectory in subDirectories)
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
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = isf.OpenFile(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return fileStream.Length;
                }
            }
        }
    }
}