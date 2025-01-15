namespace ConsoleApp1
{
    internal class Program
    {
        public static void print(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!"); 
            //print("123");

            //DisplFoldersFromDiskC();
            RecurseDisplFoldersFromDiskC();
        }

        // Максимальное количество вложенных папок, которое будет выведено
        // Если = 0, то без ограничения
        static int maxCountRecurse = 1;

        static bool printLvlId = false; // Печатать ли номер уровня вложенной папки?
        static bool printAccessReadFolderError = true; // Печатать ли предупреждения, когда папка недоступна для чтения?

        static void RecurseDisplFoldersFromDiskC(string rootPath = @"C:\", int currRecCount = 0)
        {
            if (currRecCount == 0)
            {
                Console.WriteLine("Все доступные папки и подпапки с диска С:\n");
            }

            try
            {
                // Получаем массив имен всех каталогов в корне диска С
                string[] directories = Directory.GetDirectories(rootPath);

                // Выводим названия всех каталогов
                foreach (string directory in directories)
                {
                    string currDirectory = Path.GetFileName(directory);

                    string spaseLvl = "";
                    for (int i = 0; i < currRecCount; i++) 
                    { 
                        spaseLvl += "  ";
                    }

                    if (printLvlId) 
                        Console.Write("lvl = " + currRecCount + " : ");

                    Console.WriteLine(spaseLvl + currDirectory);


                    if ((maxCountRecurse > 0 && currRecCount < maxCountRecurse)         // Если количество рекурсий в допустимом диапазоне
                        || (maxCountRecurse == 0))                                      // Или если нет ограничение на количество рекурсий
                    {
                        string newRootPath = Path.Combine(rootPath, currDirectory);     // Построение нового пути
                        RecurseDisplFoldersFromDiskC(newRootPath, currRecCount + 1);    // Рекурсивный вызов с новыми аргументами
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Игнорируем папки, к которым нет доступа
                if(printAccessReadFolderError) 
                    Console.WriteLine($"\n🛑 Нет доступа к папке: {rootPath}\n");
            }
        }

        static void DisplFoldersFromDiskC(string rootPath = @"C:\")
        {
            // Получаем массив имен всех каталогов в корне диска С
            string[] directories = Directory.GetDirectories(rootPath);

            // Выводим названия всех каталогов
            foreach (string directory in directories)
            {
                Console.WriteLine(Path.GetFileName(directory));
            }
        }
    }
}
