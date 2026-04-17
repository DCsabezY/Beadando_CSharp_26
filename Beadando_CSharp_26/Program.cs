using System;
using System.ComponentModel.Design;
using System.Threading;

namespace Beadando_CSharp_26
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MovieProcessor processor = new MovieProcessor();

            Thread consumerThread = new Thread(processor.ConsumerWork);
            consumerThread.Start();
            bool running = true;

            while (running)
            {
                string choice = ShowMenu();

                switch (choice)
                {
                    case "1":
                        AddMovieManually(processor);
                        break;
                    case "2":
                        LoadMoviesFromFile(processor);
                        break;
                    case "3":
                        processor.Queue.PrintAllMovies();
                        break;
                    case "4":
                        PrintMoviesByGenre(processor);
                        break;
                    case "5":
                        PrintMoviesByLength(processor);
                        break;
                    case "6":
                        processor.Queue.CountMovies();
                        break;
                    case "7":
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

            processor.Stop();
            consumerThread.Join();
            processor.Close();

            Console.WriteLine("Program finished.");
        }

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
        static void AddMovieManually(MovieProcessor processor)
        {
            Console.Write("Movie title: ");
            string? title = Console.ReadLine();

            Console.Write("Movie length (minutes): ");
            string? lengthInput = Console.ReadLine();

            Console.Write("Movie genre: ");
            string? genre = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(genre))
            {
                Console.WriteLine("Title and genre cannot be empty.");
                return;
            }

            if (!int.TryParse(lengthInput, out int length) || length <= 0)
            {
                Console.WriteLine("Invalid movie length.");
                return;
            }

            Movies movie = new Movies(title, length, genre);
            processor.AddMovie(movie);
        }

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
            processor.Queue.PrintMoviesByGenre(genre);
        }

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
            processor.Queue.PrintMoviesByLength(length);
        }
    }
}