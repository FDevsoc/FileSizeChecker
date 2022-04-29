namespace FileSizeChecker.Controllers
{
    public class CommandLineContext
    {
        public string Path { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public bool Quite { get; set; }
        public bool Humanread { get; set; }
    }
}

