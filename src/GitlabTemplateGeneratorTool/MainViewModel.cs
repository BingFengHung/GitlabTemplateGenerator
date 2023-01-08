using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GitlabTemplateGeneratorTool
{
    class MainViewModel : ViewModelBase
    {
        public string FromUri { get; set; }

        public string ToUri { get; set; }

        public string DownloadPath { get; set; }

        public ICommand CloneClickCommand => new RelayCommand((i) =>
        {
            Clone_Click();
        });

        public ICommand SelectDownloadPathCommand => new RelayCommand((i) =>
        {
            DownloadPath_Select();
        });

        public void DownloadPath_Select()
        {
            var ookiDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            if (ookiDialog.ShowDialog() == true)
            {
                DownloadPath = ookiDialog.SelectedPath;
            }
        }

        public void Clone_Click()
        {
            Uri fromUri = new Uri(FromUri);
            Uri toUri = new Uri(ToUri);

            string source = Path.Combine("./", fromUri.Segments[2].Replace(".git", ""));
            string destination = Path.Combine(DownloadPath, toUri.Segments[2].Replace(".git", ""));

            GitClone(fromUri.AbsoluteUri, source);
            GitClone(toUri.AbsoluteUri, destination);

            try
            {
                Copy(source, destination);
                // RemoveDir(source);
                DeleteFolder(source);
                System.Windows.MessageBox.Show("Success!");
            }
            catch
            {
                System.Windows.MessageBox.Show("Fail!");
            }
        }

        static void DeleteFolder(string path)
        {
            foreach (string d in Directory.GetFileSystemEntries(path))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);
                }
                else DeleteFolder(d);
            }

            var di = new DirectoryInfo(path);
            di.Attributes = FileAttributes.Normal;
            Directory.Delete(path, true);
        }

        public static void GitClone(string url, string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = $@"/C git clone {url} ""{path}""";
            process.Start();
            process.WaitForExit();
        }


        public static void RemoveDir(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.RedirectStandardOutput = false;
            // process.StartInfo.CreateNoWindow = false;
            process.StartInfo.Arguments = $@"/C rmdir /s /q ""{path}""";
            process.Start();
            process.WaitForExit();
        }

        public void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                if (diSourceSubDir.Name == ".git") continue;
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
