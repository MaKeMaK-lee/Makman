﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace Makman.Data.WindowsOS
{
    static internal class WindowsUse
    {
        static internal string? ChooseDirectory()
        {
            OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
            dialog.Title = "Select a directory";

            bool? result = dialog.ShowDialog();

            if (result == true)
                return dialog.FolderName;
            else
                return null;
        }

        static internal IEnumerable<string> GetDirectoriesFromDirectory(string directoryPath)
        {
            return Directory.EnumerateDirectories(directoryPath);
        }

        static internal IEnumerable<string> GetFilesFromDirectory(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath);
        }

        static internal IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, "*", System.IO.SearchOption.AllDirectories);
        }

        static internal void ViewInExplorer(string path)
        {
            path.Replace("\\", "/");
            Process.Start(new ProcessStartInfo { FileName = "explorer", Arguments = $"/n, /select, {path}" });
        }

        static internal bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        static internal string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        static internal void WriteAllText(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        static internal DateTime GetFileLastWriteTime(string path)
        {
            return new FileInfo(path).LastWriteTime;
        }

        static internal long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }

        static internal void RenameFile(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        static internal void FilesMoveToDirectory(IEnumerable<string> filePaths, string directoryPath, Action<string, bool>? statusUpdateAction)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            int count = filePaths.Count();
            int moved = 0;
            foreach (var filePath in filePaths)
            {
                statusUpdateAction?.Invoke("Files moved: " + moved + " / " + count + "\n\n" +
                    "Current source: " + filePath + "\n\n" +
                    "Current targrt directory: " + directoryPath, false);
                File.Move(filePath, directoryPath + "\\" + filePath.Split("\\").Last());
                moved++;
            }
            statusUpdateAction?.Invoke("Files moved: " + moved + " / " + count + "\n\n" +
                 "Current targrt directory: " + directoryPath, true);
        }

        static internal void FilesMoveToDirectorySlowly(IEnumerable<string> filePaths, string directoryPath, Action<string, bool>? statusUpdateAction, long averageSpeedByKBpS, int msWaitingBetweenFiles)
        {
            int count = filePaths.Count();
            int moved = 0;
            string statusString = "";
            foreach (var filePath in filePaths)
            {
                statusString = "Files moved: " + moved + " / " + count + "\n\n" +
                    "Current source: " + filePath + "\n\n" +
                    "Current targrt directory: " + directoryPath;

                int msPerIteration = 50;
                long fileSizeKB = GetFileSize(filePath) / 1024;
                int fileWaitingTime = ((int)(1000 * fileSizeKB / averageSpeedByKBpS)) + msWaitingBetweenFiles;

                long timeSleeped = 0;
                if (moved != 0)
                {
                    for (; timeSleeped < fileWaitingTime; timeSleeped += msPerIteration)
                    {
                        statusUpdateAction?.Invoke(statusString + "\n\n" +
                            $"Waiting: {(double)timeSleeped / 1000:f2} / {(double)fileWaitingTime / 1000:f2} с", false);
                        Thread.Sleep(msPerIteration);
                    }
                }
                File.Move(filePath, directoryPath + "\\" + filePath.Split("\\").Last());

                moved++;
                statusString = "Files moved: " + moved + " / " + count + "\n\n" +
                    "Current source: " + filePath + "\n\n" +
                    "Current targrt directory: " + directoryPath;

                statusUpdateAction?.Invoke(statusString + "\n\n", false);
            }
            statusUpdateAction?.Invoke(statusString + "\n\n", true);
        }

        static internal BitmapImage? TryGetImageOfFile(string path)
        {
            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path);
                image.EndInit();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        static internal object GetImageOfFile(string path, bool justThumbnail)
        {
            using (var shellObject = ShellObject.FromParsingName(path))
            {
                if (!justThumbnail)
                {
                    string contentType = (string)shellObject.Properties.GetProperty("System.ContentType").ValueAsObject;
                    if (contentType?.StartsWith("image") ?? false)
                    {
                        var imageResult = TryGetImageOfFile(path);
                        if (imageResult != null)
                        {
                            return imageResult;
                        }
                    }
                    return shellObject.Thumbnail.ExtraLargeBitmapSource;
                }
                return shellObject.Thumbnail.MediumBitmapSource;
            }
        }
    }
}
