using System;
using System.Collections.Generic;
using System.IO;
using Dominion.Utility.Constants;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Io
{
    /// <summary>
    /// note: jay: this is a library I've used over the years.
    /// I copied this from an old project.
    /// This will have to be tested at some point but this will primarily be used for unit testing and other utilitarian activities.
    /// </summary>
    public class DsIo
    {
        /// <summary>
        /// Get the directory of the executable with no ending slash.
        /// </summary>
        /// <param name="obj">Any object. This may seem weird but it almost ensures it's always available.</param>
        /// <returns>Directory path string with no ending slash.</returns>
        public static string GetExecutingDirectory()
        {
            return GetExecutingDirectory(string.Empty);
        }

        /// <summary>
        /// Get the directory of the executable and append a file name w/extension to it.
        /// </summary>
        /// <param name="obj">Any object. This may seem weird but it almost ensures it's always available.</param>
        /// <returns>Directory path string with no ending slash.</returns>
        public static string GetExecutingDirectory(string fileNameWithExtension)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            // remove slashes from the app path if there are any
            appPath = appPath.TrimEnd(new[] { '\\' });

            // create the path using the file name
            string path =
                string.IsNullOrEmpty(fileNameWithExtension)
                    ? appPath
                    : string.Format("{0}\\{1}", appPath, fileNameWithExtension);

            return path;
        }

        /// <summary>
        /// Remove the number of specified directories from the path.
        /// If no file is being added it will return the path without a slash at the end.
        /// If a file is being added it will end with a valid path format with file at the end.
        /// </summary>
        /// <param name="path">The path you're modifying.</param>
        /// <param name="dirsToRemove">How many directories to remove from the path.</param>
        /// <param name="fileWithExtToAdd">
        /// Optional. If included it will return a valid file path with file specified.
        /// </param>
        /// <returns>A path to either a directory or file.</returns>
        public static string RemoveDirectoriesFromPath(
            string path, 
            int dirsToRemove, 
            string fileWithExtToAdd = CommonConstants.EMPTY_STRING)
        {
            while(true)
            {
                if(string.IsNullOrEmpty(path) || dirsToRemove < 1)
                    return path;

                string parent = Path.GetDirectoryName(path);

                if(--dirsToRemove > 0)
                {
                    path = parent;
                    continue;
                }

                return
                    string.IsNullOrEmpty(fileWithExtToAdd)
                        ? path
                        : path + "\\" + fileWithExtToAdd;
            }
        }

        public static string IfNoDirectoryCreateIt(string path)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static void CopyDirectories(string source, string target)
        {
            string[] dirs = Directory.GetDirectories(source, "*.*", SearchOption.AllDirectories);
            foreach(string sourceDir in dirs)
            {
                string targetDir = sourceDir.Replace(source, target);
                Directory.CreateDirectory(targetDir);
            }
        }

        public static void CopyDirectoriesFiles(string source, string target)
        {
            string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            foreach(string sourceFilePath in files)
            {
                string targetFilePath = sourceFilePath.Replace(source, target);
                File.Copy(sourceFilePath, targetFilePath, true);
                new FileInfo(targetFilePath).Attributes = FileAttributes.Normal; // remove read-only
            }
        }

        public static string CopyFileToTempFolder(string path)
        {
            string tmpPath = Path.GetTempFileName();
            FileInfo source = new FileInfo(path);
            source.CopyTo(tmpPath, true);
            return tmpPath;
        }

        public static void CopyDirectoriesFiles(
            string source, 
            string target, 
            string filter, 
            bool overWrite, 
            SearchOption searchOption, 
            List<string> exclusionList)
        {
            string[] files = Directory.GetFiles(source, filter, searchOption);
            foreach(string sourceFilePath in files)
            {
                bool skipFile = false;

                // Skip items user wants excluded
                foreach(string exclusion in exclusionList)
                {
                    if(sourceFilePath.Contains(exclusion))
                    {
                        skipFile = true;
                        break;
                    }
                }

                if(skipFile)
                    continue;

                string targetFilePath = sourceFilePath.Replace(source, target);
                File.Copy(sourceFilePath, targetFilePath, overWrite);
                new FileInfo(targetFilePath).Attributes = FileAttributes.Normal; // remove read-only
            }
        }

        public static string FileSaveDateTimeString()
        {
            string format = "[yyyy-MM-dd][HH-mm-ss]";
            return DateTime.Now.ToString(format);
        }

        public static string GenerateTempFilePathPrependDateTimeStamp(string fileNameWithExtension)
        {
            string dirPath = Path.GetTempPath();
            IfNoDirectoryCreateIt(dirPath);
            string path = string.Format(
                "{0}{1}-{2}", 
                dirPath, 
                FileSaveDateTimeString(), 
                fileNameWithExtension);
            return path;
        }

        public static string GenerateFilePathPrependDateTimeStamp(string dirPath, string fileNameWithExtension)
        {
            dirPath = dirPath.TrimEnd(new[] { '\\' });
            IfNoDirectoryCreateIt(dirPath);
            string path = string.Format(
                "{0}\\{1}-{2}", 
                dirPath, 
                FileSaveDateTimeString(), 
                fileNameWithExtension);
            return path;
        }

        public static string GenerateFilePath(string dirPath, string fileNameWithExtension)
        {
            dirPath = dirPath.TrimEnd(new[] { '\\' });
            IfNoDirectoryCreateIt(dirPath);
            string path = string.Format(
                "{0}\\{1}", 
                dirPath, 
                fileNameWithExtension);
            return path;
        }

        public static bool? IsFile(string path)
        {
            if(File.Exists(path))
                return true;

            if(Directory.Exists(path))
                return false;
                
            return null;
        }

        public static string CopyFileToNewDirectory(string source, string destinationDir)
        {
            if(!destinationDir.EndsWith("\\"))
            {
                destinationDir = destinationDir+"\\";
            }

            string destPath = string.Format("{0}{1}", destinationDir, Path.GetFileName(source));
            File.Copy(source, destPath, true);
            return destPath;
        }

        public static string CopyFileToNewDirectory(string source, string destinationDir, string newFileName)
        {
            if(!destinationDir.EndsWith("\\"))
            {
                destinationDir = destinationDir+"\\";
            }

            string destPath = string.Format("{0}{1}", destinationDir, newFileName);
            File.Copy(source, destPath, true);
            return destPath;
        }

        public static void DeleteFileOlderThanMonths(string dirPath, int months)
        {
            string[] files = Directory.GetFiles(dirPath);

            foreach(var file in files)
            {
                var fi = new FileInfo(file);
                if(fi.LastAccessTime < DateTime.Now.AddMonths(months * -1))
                    fi.Delete();
            }
        }

        public static IOpResult DeleteLocalFile(string path)
        {
            var result = new OpResult.OpResult();

            if (File.Exists(path))
                result.TryCatch(() => File.Delete(path));
            else
                result.TryCatch(() => { throw new FileNotFoundException("File not found.", path); });

            return result;
        }

    }
}
