using System.IO.IsolatedStorage;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    /// <summary>
    /// IsolatedStorageFile 扩展类。
    /// </summary>
    public static class IsolatedStorageFileExtensions
    {
        /// <summary>
        /// 递归删除独立存储根目录的某个文件夹及其子级内容。
        /// </summary>
        /// <param name="directory">需要删除的根目录文件夹名称。</param>
        public static void DeleteDirectoryRecursive(string directory)
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            if (iso.DirectoryExists(directory))
            {
                string[] files = iso.GetFileNames(directory + @"/*");
                foreach (string file in files)
                {
                    iso.DeleteFile(directory + @"/" + file);
                }

                string[] subDirectories = iso.GetDirectoryNames(directory + @"/");
                foreach (string subDirectory in subDirectories)
                {
                    DeleteDirectoryRecursive(directory + @"/" + subDirectory);
                }

                iso.DeleteDirectory(directory);
            }
        }
    }
}