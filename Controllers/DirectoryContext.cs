using FileSizeChecker.Models;

namespace FileSizeChecker.Controllers
{
    public class DirectoryContext
    {
        public static string FileLog { get; set; } = string.Empty;

        static void GetFiles(Folder folder)
        {
            string[] filePaths = Directory.GetFiles(folder.Path);
            foreach (string filePath in filePaths)
            {
                var file = new Models.File(filePath);
                folder.Files.Add(file);
            }
        }

        static void GetSize(Folder folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder.Path);
            FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            folder.Size = files.Sum(f => f.Length);
        }

        static Folder GetSubFolders(Folder folder)
        {
            GetSize(folder);
            GetFiles(folder);

            string[] subFolderPaths = Directory.GetDirectories(folder.Path);
            foreach (string subFolderPath in subFolderPaths)
            {
                Folder subFolder = new Folder(subFolderPath);
                folder.SubFolders.Add(GetSubFolders(subFolder));
            }

            return folder;
        }

        public static void GetNestingLevel(Folder folder, string currentLevel)
        {
            foreach (var file in folder.Files)
            {
                file.NestingLevel += "--";
            }

            foreach (var subFolder in folder.SubFolders)
            {
                subFolder.NestingLevel += currentLevel + "--";

                foreach (var file in subFolder.Files)
                {
                    file.NestingLevel = subFolder.NestingLevel + "--";
                }

                GetNestingLevel(subFolder, subFolder.NestingLevel);
            }
        }


        static List<Folder> WriteDirectoryItems(Folder folder, bool humanRead)
        {
            var subFolderLink = folder;

            foreach (var subFolder in folder.SubFolders)
            {
                if (humanRead)
                    FileLog += $"{subFolder.NestingLevel} {subFolder.Name} ({FileContext.SizeSuffix(subFolder.Size, 2)})\n";
                else
                    FileLog += $"{subFolder.NestingLevel} {subFolder.Name} ({subFolder.Size} bytes)\n";

                WriteDirectoryItems(subFolder, humanRead);
            }
            foreach (var file in subFolderLink.Files)
            {
                if (humanRead)
                    FileLog += $"{file.NestingLevel} {file.Name} ({FileContext.SizeSuffix(file.Size, 2)})\n";
                else
                    FileLog += $"{file.NestingLevel} {file.Name} ({file.Size} bytes)\n";
            }

            return folder.SubFolders;
        }

        static public void WriteDirectoryTree(string path, bool isQuite, bool humanRead)
        {

            Folder folder = new Folder(path);
            folder = GetSubFolders(folder);
            GetNestingLevel(folder, folder.NestingLevel);
            var directoryTree = new List<Folder>() { folder };

            if (humanRead)
                FileLog += $"- <{folder.Name}> ({FileContext.SizeSuffix(folder.Size, 2)})\n";
            else
                FileLog += $"- <{folder.Name}> ({folder.Size} bytes)\n";

            foreach (var item in directoryTree)
            {
                WriteDirectoryItems(item, humanRead);
            }

            if (isQuite)
                Console.Write(FileLog);
        }
    }
}