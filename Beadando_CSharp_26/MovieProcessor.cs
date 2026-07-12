using System;
using System.IO;
using System.Threading;

namespace Beadando_CSharp_26
{
    // Producer-consumer minta: a főszál adja hozzá a filmeket, egy háttérszál dolgozza fel őket.
    internal class MovieProcessor
    {
        private OwnConcurrentQueue _queue;      // feldolgozásra váró filmek
        private OwnConcurrentQueue _allMovies;  // minden film, listázáshoz (ebből nem vesz ki a consumer)
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
            _queue.Add(movie);
            _allMovies.Add(movie);
            Console.WriteLine($"Added: {movie.MovieTitle}");

            _movieAddedEvent.EventSet(); // felébreszti a consumer szálat
        }

        public void ConsumerWork()
        {
            while (_isRunning || !_queue.IsEmpty())
            {
                _movieAddedEvent.EventWait(); // alszik, amíg nincs film

                // egyszerre az összes elérhető filmet feldolgozza, nem csak egyet
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
            Thread.Sleep(500); // feldolgozási idő szimulációja
        }

        // Formátum soronként: Cím;Hossz(perc);Műfaj
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

                    Thread.Sleep(500); // hogy a consumer fokozatosan kapja meg a filmeket
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
            _movieAddedEvent.EventSet(); // enélkül a consumer örökre blokkolna (deadlock)
        }

        public void Close()
        {
            _movieAddedEvent.EventClose(); // csak a consumer leállása UTÁN hívható
        }

        public OwnConcurrentQueue Queue => _queue;
        public OwnConcurrentQueue AllMovies => _allMovies;
    }
}