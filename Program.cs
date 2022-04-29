using FileSizeChecker.Controllers;
using Fclp;

namespace FileSizeChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectoryPath = Environment.CurrentDirectory;
            string defaultLogFilePath = FileContext.GetDefaultFilePath();

            var p = new FluentCommandLineParser<CommandLineContext>();
            p.Setup(arg => arg.Path)
             .As('p', "path")
             .SetDefault(currentDirectoryPath)
             .Callback(item => p.Object.Path = item)
             .WithDescription("Укажите директорию для анализа");

            p.Setup(arg => arg.Output)
             .As('o', "output")
             .SetDefault(defaultLogFilePath)
             .Callback(item => p.Object.Output = item)
             .WithDescription("Укажите файл, в который запишется лог");

            p.Setup(arg => arg.Quite)
             .As('q', "quite")
             .SetDefault(true)
             .WithDescription("Не выводить лог в консоль");

            p.Setup(arg => arg.Humanread)
             .As('h', "humanread")
             .SetDefault(false)
             .WithDescription("Преобразовать размеры в читаемую форму");

            var result = p.Parse(args);


            if (!Directory.Exists(p.Object.Path))
            {
                Console.WriteLine("Ошибка! Неверно указан путь к директории для анализа!");
                Environment.Exit(0);
            }
            else if (!File.Exists(p.Object.Output) && p.Object.Output != defaultLogFilePath)
            {
                Console.WriteLine("Ошибка! Неверно указан путь к файлу записи лога!");
                Environment.Exit(0);
            }

            if (result.HasErrors)
            {
                Console.WriteLine("Ошибка! Некорректно задан один или несколько параметров.\n\t\t\tFAQ");
                foreach (var item in p.Options)
                {
                    Console.WriteLine($"--{item.LongName}, -{item.ShortName} | {item.Description}");
                }
                Environment.Exit(0);
            }

            if (args.Contains("-q") || args.Contains("--quite"))
                p.Object.Quite = false;

            if (args.Contains("-h") || args.Contains("--humanread"))
                p.Object.Humanread = true;

            try
            {
                DirectoryContext.WriteDirectoryTree(p.Object.Path, p.Object.Quite, p.Object.Humanread);

                if (args.Contains("-o") || args.Contains("--output"))
                  FileContext.WriteLogInFile(p.Object.Output);
                else
                FileContext.DefaultWriteLog();
            }
            catch
            {
                Console.WriteLine("Ошибка доступа к запрашиваемому пути. " +
                                  "Перейдите в корневой каталог программы.");
                Environment.Exit(0);
            }
        }
    }
}
