using System;
using System.ComponentModel.Design;
using System.Threading;

namespace Beadando_CSharp_26
{
    /// <summary>
    /// A program belépési pontja.
    /// 
    /// Két szál fut párhuzamosan:
    ///   - Főszál:        a menüt kezeli, filmeket vesz fel (producer szerepe)
    ///   - consumerThread: feldolgozza a filmeket a háttérben (consumer szerepe)
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            MovieProcessor processor = new MovieProcessor();

            // Consumer szál létrehozása és indítása.
            // A ConsumerWork() azonnal várakozni kezd a jelzőeszközön,
            // amíg a főszál (producer) filmet nem ad hozzá.
            Thread consumerThread = new Thread(processor.ConsumerWork);
            consumerThread.Start();

            bool running = true;

            // Főszál: menüt jelenít meg és kezeli a felhasználói bevitelt.
            // Ez a producer oldal – itt keletkeznek a filmek.
            while (running)
            {
                string choice = ShowMenu();

                switch (choice)
                {
                    case "1":
                        // Manuális filmfelvétel: a felhasználó egyenként adja meg az adatokat
                        AddMovieManually(processor);
                        break;
                    case "2":
                        // Fájlból való betöltés: soronként olvassa be és adja hozzá a filmeket
                        LoadMoviesFromFile(processor);
                        break;
                    case "3":
                        // Az összes film kilistázása a tartós tárolóból (_allMovies)
                        processor.AllMovies.PrintAllMovies();
                        break;
                    case "4":
                        PrintMoviesByGenre(processor);
                        break;
                    case "5":
                        PrintMoviesByLength(processor);
                        break;
                    case "6":
                        // A tartós tárolóban lévő filmek megszámlálása
                        processor.AllMovies.CountMovies();
                        break;
                    case "7":
                        // Mindkét sort törli: a feldolgozót és a tartósat is
                        processor.AllMovies.ClearQueue();
                        processor.Queue.ClearQueue();
                        Console.WriteLine("Queue cleared.");
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid menu option.");
                        break;
                }
            }

            // Leállítási sorrend – FONTOS, hogy ebben a sorrendben történjen:

            // 1. Stop(): jelzi a consumernek, hogy ne várjon több filmre
            //    (ha ezt kihagynánk, a consumerThread örökre blokkolna -> deadlock)
            processor.Stop();

            // 2. Join(): megvárjuk, amíg a consumer szál teljesen leáll.
            //    Nélküle a program bezárhatná a jelzőeszközt, mielőtt
            //    a consumer befejezte volna a munkáját.
            consumerThread.Join();

            // 3. Close(): csak miután a consumer leállt, szabadítjuk fel
            //    a jelzőeszköz erőforrásait – biztonságos sorrend.
            processor.Close();

            Console.WriteLine("Program finished.");
        }

        /// <summary>
        /// Megjeleníti a menüt és visszaadja a felhasználó választását.
        /// </summary>
        static string ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== MOVIE MENU ===");
            Console.WriteLine("1 - Add movie manually");
            Console.WriteLine("2 - Load movies from file");
            Console.WriteLine("3 - Print all movies in queue");
            Console.WriteLine("4 - Print movies by genre");
            Console.WriteLine("5 - Print movies by length");
            Console.WriteLine("6 - Count movies in queue");
            Console.WriteLine("7 - Clear queue");
            Console.WriteLine("0 - Exit");
            Console.Write("Choose an option: ");

            return Console.ReadLine() ?? "";
        }

        /// <summary>
        /// Bekéri a film adatait a felhasználótól és hozzáadja a feldolgozóhoz.
        /// Validálja a bevitelt: cím és műfaj nem lehet üres,
        /// a hossznak pozitív egész számnak kell lennie.
        /// </summary>
        static void AddMovieManually(MovieProcessor processor)
        {
            Console.Write("Movie title: ");
            string? title = Console.ReadLine();

            Console.Write("Movie length (minutes): ");
            string? lengthInput = Console.ReadLine();

            Console.Write("Movie genre: ");
            string? genre = Console.ReadLine();

            // Üres cím vagy műfaj nem fogadható el
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(genre))
            {
                Console.WriteLine("Title and genre cannot be empty.");
                return;
            }

            // A hossznak számnak kell lennie és pozitívnak
            if (!int.TryParse(lengthInput, out int length) || length <= 0)
            {
                Console.WriteLine("Invalid movie length.");
                return;
            }

            Movies movie = new Movies(title, length, genre);
            // AddMovie: berakja a sorba ÉS jelzést küld a consumer szálnak
            processor.AddMovie(movie);
        }

        /// <summary>
        /// Bekéri a fájl elérési útját, majd átadja a processornak betöltésre.
        /// </summary>
        static void LoadMoviesFromFile(MovieProcessor processor)
        {
            Console.Write("File path: ");
            string? fileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("File path cannot be empty.");
                return;
            }

            processor.LoadMoviesFromFile(fileName);
        }

        /// <summary>
        /// Bekéri a műfajt és kilistázza a hozzá tartozó filmeket.
        /// </summary>
        static void PrintMoviesByGenre(MovieProcessor processor)
        {
            Console.Write("Genre: ");
            string? genre = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(genre))
            {
                Console.WriteLine("Genre cannot be empty.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"=== MOVIES IN GENRE: {genre} ===");
            processor.AllMovies.PrintMoviesByGenre(genre);
        }

        /// <summary>
        /// Bekéri a maximum hosszt és kilistázza az annál nem hosszabb filmeket.
        /// </summary>
        static void PrintMoviesByLength(MovieProcessor processor)
        {
            Console.Write("Maximum length: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int length) || length <= 0)
            {
                Console.WriteLine("Invalid length.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"=== MOVIES WITH LENGTH <= {length} ===");
            processor.AllMovies.PrintMoviesByLength(length);
        }
    }
}