using System;
using System.Diagnostics;
using System.Security.Principal;

namespace ConsoleApp1
{
    internal class Program
    {
        // Написал свои процедуры для вывода текста в консоль
        public static void print(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                OutTextToTxtFiles += arg + "\n";    // Дополнительно весь текст, который выводится - буферизуется, и дальше записывается в текстовый файл
                AllShowStrings++;                   // Также считаются значения для статистики
            }
        }        

        public static void print_adjacent(params object[] args)
        {
            // Печать без переноса строки
            foreach (var arg in args)
            {
                Console.Write(arg);
                OutTextToTxtFiles += arg;
            }
        }

        static void Main(string[] args)
        {
            // Создаем объект Stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Запускаем таймер
            stopwatch.Start();

            UndoStatsPrint();

            // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль
            RecurseDisplFoldersFromDiskC();

            // Останавливаем таймер
            stopwatch.Stop();

            PrintStats();

            if (stopwatch.ElapsedMilliseconds / 1000 > 120)
            {
                // Выводим время выполнения в минутах
                print("\nВремя выполнения: " + Math.Round((double)(stopwatch.ElapsedMilliseconds / 1000 / 60), 2) + " минут\n");
            }
            else
            {
                // Выводим время выполнения в секундах
                print("\nВремя выполнения: " + stopwatch.ElapsedMilliseconds / 1000 + " секунд\n");
            }         

            SaveTextToFile();            
        }

        // Настраиваемые параметры:

        // Максимальное количество вложенных папок, которое будет выведено
        // Если = 0, то без ограничения
        static int maxCountRecurse = 1; ////////////////////////////////////// Потом поставить 0

        static bool printLvlId = false;                 // Печатать ли номер уровня вложенной папки?
        static bool printAccessReadFolderError = true;  // Печатать ли предупреждения, когда папка недоступна для чтения?
        static bool whyPrintSpaseLvl = true;            // Выводить пробелы как уровни для папок? (если = false), то они будут выводится со спецсимволами типо └──

        public static string OutTextToTxtFiles = "";    // Текст, для вывода в тестовый .txt файл

        // Статистика:

        public static int AllShowStrings = 0;           // Всего строк было выведено
        public static int MaxLvlFromRecurse = 0;        // Максимальный уровень рекурсии (кол-ва вложенных папок друг в друга)

        // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль
        static void RecurseDisplFoldersFromDiskC(string rootPath = @"C:\", int currRecCount = 0)
        {
            try
            {
                // Получаем массив имен всех каталогов в корне диска С
                string[] directories = Directory.GetDirectories(rootPath);

                if(currRecCount > MaxLvlFromRecurse) MaxLvlFromRecurse = currRecCount;

                // Выводим названия всех каталогов
                foreach (string directory in directories)
                {
                    string currDirectory = Path.GetFileName(directory);

                    string spaseLvl = "";
                    for (int i = 0; i < currRecCount*2; i++) 
                    {
                        if (whyPrintSpaseLvl == true)
                        {
                            spaseLvl += " ";
                        }
                        else
                        {
                            if (i == 0) spaseLvl = "└";
                            else spaseLvl += "─";
                        }
                    }

                    if (printLvlId)
                        print_adjacent("lvl = " + currRecCount + " : ");

                    print(spaseLvl + currDirectory);


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
                    print($"\n🛑 Нет доступа к папке: {rootPath}\n");
            }
        }

        // Это выводится перед основной процедурой
        public static void UndoStatsPrint()
        {           
            // Выводим текущую время и дату
            print("\nТекущая дата и время: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");

            // Получаем текущее имя пользователя
            string username = Environment.UserName;

            // Получаем права текущего пользователя
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            string permissions = principal.IsInRole(WindowsBuiltInRole.Administrator) ? "Администратор" : "Обычный пользователь";

            Console.WriteLine("Выполняем сканирование диска C от имени пользователя: " + username + ", его права: " + permissions + "\n");

            if (maxCountRecurse != 0) 
            {
                print("Ограничение рекурсии: Максимум " + maxCountRecurse + " уровень" + "\n");
            }

            print("Все доступные папки и подпапки с диска С:\n");

            print("_______________________\n");
        }

        // Выводит статистику по количеству строк и максимально глубокому уровню
        public static void PrintStats() 
        {
            print("_______________________");
            print("\nВсего выведено строк: " + AllShowStrings);
            print("\nМаксимальный уровень рекурсии: " + MaxLvlFromRecurse);
        }

        public static void SaveTextToFile()
        {
            string text = OutTextToTxtFiles;
            string fileName = "Out_1.txt";

            try
            {
                // Получаем путь к директории выполнения программы
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                // Объединяем путь директории и имя файла
                string filePath = Path.Combine(directoryPath, fileName);

                // Записываем текст в файл
                File.WriteAllText(filePath, text);
                print("Вывод успешно сохранен в файл, по пути: " + filePath);
            }
            catch (Exception ex)
            {
                print("Произошла ошибка при сохранении текста в файл: " + ex.Message);
            }
        }

        static void DisplFoldersFromDiskC(string rootPath = @"C:\")
        {
            // Получаем массив имен всех каталогов в корне диска С
            string[] directories = Directory.GetDirectories(rootPath);

            // Выводим названия всех каталогов
            foreach (string directory in directories)
            {
                print(Path.GetFileName(directory));
            }
        }
    }
}
