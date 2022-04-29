namespace FileSizeChecker.Models
{
    public class File
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public string FolderPath { get; set; } = string.Empty;
        public string NestingLevel { get; set; } = string.Empty;

        public File(string Path)
        {
            this.Path = Path;
            string[] pathItems = Path.Split('\\');
            Name = pathItems[pathItems.Length - 1];
            Size = new FileInfo(Path).Length;
            FolderPath = Path.Replace(Name, string.Empty);
        }
    }
}
