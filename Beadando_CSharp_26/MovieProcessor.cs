using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Beadando_CSharp_26
{
    /// <summary>
    /// A filmek feldolgozásáért felelős osztály.
    /// 
    /// Producer-Consumer mintát valósít meg:
    ///   - Producer: a főszál adja hozzá a filmeket (AddMovie / LoadMoviesFromFile)
    ///   - Consumer: egy háttérszál dolgozza fel őket (ConsumerWork)
    /// 
    /// A két szál összehangolását az OwnAutoResetEvent végzi:
    /// a consumer alszik, amíg nincs film – a producer felkelti, ha van.
    /// </summary>
    internal class MovieProcessor
    {
        // Feldolgozásra váró filmek sora – a consumer ebből vesz ki és dolgoz fel.
        // Ha feldolgozta, az elem végleg eltűnik innen.
        private OwnConcurrentQueue _queue;

        // Tartós tároló – ide is bekerül minden film, de innen nem vesz ki a consumer.
        // Ez biztosítja, hogy listázáskor (pl. műfaj szerint) mindig látszanak a filmek,
        // akkor is, ha a _queue már feldolgozta és törölte őket.
        private OwnConcurrentQueue _allMovies;

        // Jelzőeszköz a két szál között:
        // A producer Set()-tel jelez, a consumer Wait()-tel vár.
        // AutoResetEvent: minden Set() pontosan egy Wait()-et enged át.
        private OwnAutoResetEvent _movieAddedEvent;

        // Amíg true, a consumer szál fut.
        // Stop() hívásra false lesz, és a consumer leáll.
        private bool _isRunning;

        /// <summary>
        /// Inicializálja a sorokat, a jelzőeszközt, és elindítja a futást.
        /// </summary>
        public MovieProcessor()
        {
            _queue = new OwnConcurrentQueue();
            _allMovies = new OwnConcurrentQueue();
            _movieAddedEvent = new OwnAutoResetEvent();
            _isRunning = true;
        }

        /// <summary>
        /// Filmet ad hozzá a feldolgozósorhoz és a tartós tárolóhoz is,
        /// majd jelzést küld a consumer szálnak, hogy van feldolgozni való.
        /// </summary>
        /// <param name="movie">A hozzáadandó film</param>
        public void AddMovie(Movies movie)
        {
            _queue.Add(movie);      // feldolgozósorba kerül
            _allMovies.Add(movie);  // tartós tárolóba is, listázáshoz megmarad
            Console.WriteLine($"Added: {movie.MovieTitle}");

            // Felébreszti a consumer szálat
            // Az AutoResetEvent miatt automatikusan visszaáll Non-signaled-re,
            // miután a consumer egyszer átment a WaitOne()-on.
            _movieAddedEvent.EventSet();
        }

        /// <summary>
        /// A consumer szál belépési pontja.
        /// Folyamatosan fut, amíg van feldolgozandó film vagy a processor még aktív.
        /// 
        /// Működési logika:
        /// 1. Vár a jelzőeszközön (alszik, ha nincs film)
        /// 2. Jelzés esetén felébredt, kiveszi és feldolgozza az összes filmet
        /// 3. Ha _isRunning false ÉS a sor üres, kilép a ciklusból
        /// </summary>
        public void ConsumerWork()
        {
            while (_isRunning || !_queue.IsEmpty())
            {
                // Blokkolva vár, amíg a producer EventSet()-et nem hív.
                // Ha a sor üres, a szál itt "alszik" és nem fogyaszt CPU-t.
                _movieAddedEvent.EventWait();

                // Felébred – kiveszi és feldolgozza az összes elérhető filmet.
                // TakeOutMovie szálbiztos: ConcurrentQueue.TryDequeue()-t használ.
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

        /// <summary>
        /// Egy film feldolgozása – kiírja az adatait, majd vár 500ms-t,
        /// szimulálva egy valódi feldolgozási műveletet.
        /// </summary>
        /// <param name="movie">A feldolgozandó film</param>
        private void ProcessMovie(Movies movie)
        {
            Console.WriteLine($"Processing: {movie}");
            Thread.Sleep(500); // feldolgozási idő szimulációja
        }

        /// <summary>
        /// Filmeket tölt be egy fájlból soronként.
        /// Minden sor formátuma: Cím;Hossz(perc);Műfaj
        /// 
        /// Hibakezelés:
        ///   - Ha a sor kevesebb mint 3 részből áll: kihagyja
        ///   - Ha a hossz nem szám: kihagyja
        /// </summary>
        /// <param name="fileName">A fájl elérési útja</param>
        public void LoadMoviesFromFile(string fileName)
        {
            try
            {
                using StreamReader sr = new StreamReader(fileName);
                string? line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(';');

                    // Ellenőrzés: legalább 3 mező kell (cím, hossz, műfaj)
                    if (lineParts.Length < 3)
                    {
                        Console.WriteLine($"Wrong line: {line}");
                        continue;
                    }

                    // Ellenőrzés: a hossz mező valóban szám-e
                    if (!int.TryParse(lineParts[1], out int length))
                    {
                        Console.WriteLine($"Invalid length: {line}");
                        continue;
                    }

                    Movies movie = new Movies(lineParts[0], length, lineParts[2]);
                    AddMovie(movie);

                    // 500ms várakozás a filmek között, hogy a consumer
                    // ne egyszerre kapja meg az összeset, hanem fokozatosan
                    Thread.Sleep(500);
                }

                Console.WriteLine("Movies loaded successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        /// <summary>
        /// Leállítja a feldolgozást:
        /// 1. _isRunning false-ra állítja
        /// 2. EventSet()-et hív – KÖTELEZŐ, különben a consumer örökre
        ///    a WaitOne()-on blokkolna, és a program soha nem érne véget (deadlock).
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _movieAddedEvent.EventSet(); // felébreszti a várakozó consumer szálat
        }

        /// <summary>
        /// Felszabadítja a jelzőeszköz erőforrásait.
        /// Mindig a consumer szál leállása UTÁN hívandó (Join() után).
        /// </summary>
        public void Close()
        {
            _movieAddedEvent.EventClose();
        }

        // Tulajdonságok a külső hozzáféréshez (Program.cs használja)
        public OwnConcurrentQueue Queue => _queue;
        public OwnConcurrentQueue AllMovies => _allMovies;
    }
}