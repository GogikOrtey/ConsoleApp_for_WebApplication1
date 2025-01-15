using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Program2
    {
        public static void print(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                OutTextToTxtFiles += arg + "\n";
                AllShowStrings++;
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

        static void OldMain(string[] args)
        {
            // Создаем объект Stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Запускаем таймер
            stopwatch.Start();


            // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль
            RecurseDisplFoldersFromDiskC();

            // Останавливаем таймер
            stopwatch.Stop();

            PrintStats();

            // Выводим текущую время и дату
            Console.WriteLine("\nТекущая дата и время: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

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
        static int maxCountRecurse = 5;

        static bool printLvlId = false;                 // Печатать ли номер уровня вложенной папки?
        static bool printAccessReadFolderError = true;  // Печатать ли предупреждения, когда папка недоступна для чтения?
        static bool whyPrintSpaseLvl = true;            // Выводить пробелы как уровни для папок? (если = false), то они будут выводится со спецсимволами типо └──

        public static string OutTextToTxtFiles = "";    // Текст, для вывода в тестовый .txt файл

        // Статистика:

        public static int AllShowStrings = 0;
        public static int MaxLvlFromRecurse = 0;

        // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль
        static void RecurseDisplFoldersFromDiskC(string rootPath = @"C:\", int currRecCount = 0)
        {
            if (currRecCount == 0)
            {
                print("Все доступные папки и подпапки с диска С:\n");
            }

            try
            {
                // Получаем массив имен всех каталогов в корне диска С
                string[] directories = Directory.GetDirectories(rootPath);

                if(currRecCount > MaxLvlFromRecurse) MaxLvlFromRecurse = currRecCount;

                // Выводим названия всех каталогов
                Parallel.ForEach(directories, directory =>
                {
                    string currDirectory = Path.GetFileName(directory);

                    string spaseLvl = "";
                    for (int i = 0; i < currRecCount * 2; i++)
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
                });
            }
            catch (UnauthorizedAccessException)
            {
                // Игнорируем папки, к которым нет доступа
                if(printAccessReadFolderError)
                    print($"\n🛑 Нет доступа к папке: {rootPath}\n");
            }
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
