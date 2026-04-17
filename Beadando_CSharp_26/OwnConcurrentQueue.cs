using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Beadando_CSharp_26
{
    internal class OwnConcurrentQueue
    {
        private ConcurrentQueue<Movies> queue;

        public OwnConcurrentQueue()
        {
            queue = new ConcurrentQueue<Movies>();
        }

        public void Add(Movies movie)
        {
            queue.Enqueue(movie);
        }

        public void PrintAllMovies()
        {
            foreach (var movie in queue)
            {
                Console.WriteLine(movie);
            }
        }

        public void PrintMoviesByGenre(string genre)
        {
            foreach (var movie in queue)
            {
                if (movie.MovieGenre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(movie);
                }
            }
        }

        public void PrintMoviesByLength(int length)
        {
            foreach (var movie in queue)
            {
                if (movie.MovieLength <= length)
                {
                    Console.WriteLine(movie);
                }
            }
        }

        public void TakeOutQueue()
        {
            while (!queue.IsEmpty)
            {
                if (queue.TryDequeue(out Movies? movie))
                {
                    Console.WriteLine($"Dequeued: {movie}");
                }
            }
        }

        public void ClearQueue()
        {
            while (!queue.IsEmpty)
            {
                queue.TryDequeue(out _);
            }
        }

        public bool TakeOutMovie(out Movies? movie)
        {
            return queue.TryDequeue(out movie);
        }

        public bool IsEmpty()
        {
            return queue.IsEmpty;
        }

        public void CountMovies()
        {
            Console.WriteLine($"Total movies in the queue: {queue.Count}");
        }


    }
}
