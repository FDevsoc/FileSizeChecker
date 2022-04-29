namespace FileSizeChecker.Models
{
    public class Folder
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<File> Files { get; set; } = new List<File>();
        public string NestingLevel { get; set; } = string.Empty;

        public Folder(string Path)
        {
            this.Path = Path;
            string[] pathItems = Path.Split('\\');
            Name = pathItems[pathItems.Length - 1];
        }
    }
}
