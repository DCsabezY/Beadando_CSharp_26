using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Beadando_CSharp_26
{
    /// <summary>
    /// Szálbiztos filmsor, amely a .NET beépített ConcurrentQueue-ját burkolja.
    /// Több szál egyidejűleg is biztonságosan olvashat és írhat belőle,
    /// külön zárolás (lock) nélkül.
    /// </summary>
    internal class OwnConcurrentQueue
    {
        // ConcurrentQueue: olyan sor, amelyet több szál egyszerre, 
        // biztonságosan lehet használni – ellentétben a sima Queue<T>-val,
        // amely versenyhelyzetet (race condition) okozhatna.
        private ConcurrentQueue<Movies> queue;

        /// <summary>
        /// Létrehozza az üres filmsort.
        /// </summary>
        public OwnConcurrentQueue()
        {
            queue = new ConcurrentQueue<Movies>();
        }

        /// <summary>
        /// Filmet ad a sor végére. (Enqueue)
        /// Szálbiztos: egyszerre több szál is hívhatja ütközés nélkül.
        /// </summary>
        public void Add(Movies movie)
        {
            queue.Enqueue(movie);
        }

        /// <summary>
        /// Kiírja a sorban lévő összes filmet a konzolra.
        /// </summary>
        public void PrintAllMovies()
        {
            foreach (var movie in queue)
            {
                Console.WriteLine(movie);
            }
        }

        /// <summary>
        /// Csak az adott műfajú filmeket írja ki. (kis-nagybetű független)
        /// </summary>
        /// <param name="genre">A keresett műfaj neve</param>
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

        /// <summary>
        /// Kiírja azokat a filmeket, amelyek hossza nem haladja meg 
        /// a megadott percet.
        /// </summary>
        /// <param name="length">Maximum hossz percben</param>
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

        /// <summary>
        /// Kiveszi és kiírja a sor összes elemét.
        /// TryDequeue: szálbiztos kivétel – visszaadja, hogy sikerült-e,
        /// így nem kell külön ellenőrizni, hogy üres-e a sor.
        /// </summary>
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

        /// <summary>
        /// Kiüríti a sort, az elemeket elveti.
        /// </summary>
        public void ClearQueue()
        {
            while (!queue.IsEmpty)
            {
                queue.TryDequeue(out _); // az elemet eldobjuk, csak kiürítjük a sort
            }
        }

        /// <summary>
        /// Megpróbál kivenni egy filmet a sor elejéről.
        /// </summary>
        /// <param name="movie">A kivett film, vagy null ha a sor üres</param>
        /// <returns>True ha sikerült kivenni, False ha üres volt a sor</returns>
        public bool TakeOutMovie(out Movies? movie)
        {
            return queue.TryDequeue(out movie);
        }

        /// <summary>
        /// Megvizsgálja, hogy a sor üres-e.
        /// </summary>
        public bool IsEmpty()
        {
            return queue.IsEmpty;
        }

        /// <summary>
        /// Kiírja a sorban lévő filmek számát.
        /// </summary>
        public void CountMovies()
        {
            Console.WriteLine($"Total movies in the queue: {queue.Count}");
        }


    }
}
