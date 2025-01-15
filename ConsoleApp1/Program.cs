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
        static int maxCountRecurse = 3;

        static void RecurseDisplFoldersFromDiskC(string rootPath = @"C:\", int currRecCount = 0)
        {
            // Получаем массив имен всех каталогов в корне диска С
            string[] directories = Directory.GetDirectories(rootPath);

            // Выводим названия всех каталогов
            foreach (string directory in directories)
            {
                string currDirectory = Path.GetFileName(directory);
                Console.WriteLine("lvl = " + currRecCount + " : " + currDirectory);

                if ((maxCountRecurse > 0 && maxCountRecurse < currRecCount) // Если количество рекурсий в допустимом диапазоне
                    || (maxCountRecurse == 0))                              // Или если нет ограничение на количество рекурсий
                {
                    rootPath += currDirectory;                              // Добавляем название текущей папки к корню  

                    currRecCount++;                                         // Добавляем уровень рекурсии
                    RecurseDisplFoldersFromDiskC(rootPath, currRecCount);   // И вызываем эту же процедуру, с новыми аргументами
                }
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
