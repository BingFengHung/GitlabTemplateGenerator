using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GitlabTemplateGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri fromUri = new Uri(@"http://192.168.168.89/joe94008/test.git");
            Uri toUri = new Uri(@"http://192.168.168.89/joe94008/SpLogCleaner.git");

            string source = $@"C:\Users\User X\Desktop\新增資料夾 (2)\{fromUri.Segments[2].Replace(".git", "")}";
            string destination = $@"C:\Users\User X\Desktop\新增資料夾 (2)\{toUri.Segments[2].Replace(".git", "")}";

            GitClone(fromUri.AbsoluteUri, source);
            GitClone(toUri.AbsoluteUri, destination);

            Copy(source, destination);
        }

        public static void GitClone(string url, string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.Arguments = $@"/C git clone {url} ""{path}""";
            process.Start();
            process.WaitForExit();
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
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
