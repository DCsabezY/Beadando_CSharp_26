using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Beadando_CSharp_26
{
    internal class MovieProcessor
    {
        private OwnConcurrentQueue _queue;
        private OwnConcurrentQueue _allMovies;
        private OwnAutoResetEvent _movieAddedEvent;
        private bool _isRunning;

        public MovieProcessor()
        {
            _queue = new OwnConcurrentQueue();
            _allMovies = new OwnConcurrentQueue();
            _movieAddedEvent = new OwnAutoResetEvent();
            _isRunning = true;
        }

        public void AddMovie(Movies movie)
        {
            _queue.Add(movie);         // feldolgozósorba
            _allMovies.Add(movie);     // tartós tárolóba is
            Console.WriteLine($"Added: {movie.MovieTitle}");
            _movieAddedEvent.EventSet();
        }

        public void ConsumerWork()
        {
            while (_isRunning || !_queue.IsEmpty())
            {
                _movieAddedEvent.EventWait();

                while (_queue.TakeOutMovie(out Movies? movie))
                {
                    if (movie != null)
                    {
                        ProcessMovie(movie);
                    }
                }
            }

            Console.WriteLine("Consumer stopped.");
        }

        private void ProcessMovie(Movies movie)
        {
            Console.WriteLine($"Processing: {movie}");
            Thread.Sleep(500);
        }

        public void LoadMoviesFromFile(string fileName)
        {
            try
            {
                using StreamReader sr = new StreamReader(fileName);
                string? line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(';');

                    if (lineParts.Length < 3)
                    {
                        Console.WriteLine($"Wrong line: {line}");
                        continue;
                    }

                    if (!int.TryParse(lineParts[1], out int length))
                    {
                        Console.WriteLine($"Invalid length: {line}");
                        continue;
                    }

                    Movies movie = new Movies(lineParts[0], length, lineParts[2]);
                    AddMovie(movie);
                    Thread.Sleep(500);
                }

                Console.WriteLine("Movies loaded successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _movieAddedEvent.EventSet(); // felébreszti a várakozó consumer threadet
        }

        public void Close()
        {
            _movieAddedEvent.EventClose();
        }

        public OwnConcurrentQueue Queue => _queue;
        public OwnConcurrentQueue AllMovies => _allMovies;
    }
}