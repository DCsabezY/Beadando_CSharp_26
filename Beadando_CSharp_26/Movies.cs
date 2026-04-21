using System;
using System.Collections.Generic;
using System.Text;

namespace Beadando_CSharp_26
{
    /// <summary>
    ///  A filmek osztálya amely tartalmazza a film címét, hosszát és műfaját, valamint egy ToString() metódust, amely visszaadja a film adatait egy formázott stringben.
    /// </summary>
    internal class Movies
    {
		private string _movieTitle;
		private int _movieLength;
		private string _movieGenre;

        public Movies(string movieTitle, int movieLength, string movieGenre)
        {
            _movieTitle = movieTitle;
            _movieLength = movieLength;
            _movieGenre = movieGenre;
        }

        /// <summary>
        /// A film műfaja, amely lehet például akció, vígjáték, dráma stb.
        /// </summary>
        public string MovieGenre
		{
			get { return _movieGenre; }
			set { _movieGenre = value; }
		}

        /// <summary>
        /// A film hossza percben kifejezve.
        /// </summary>
        public int MovieLength
		{
			get { return _movieLength; }
			set { _movieLength = value; }
		}

        /// <summary>
        /// A film címe, amely egy string érték. Ez lehet egy konkrét cím, például "Inception", vagy egy általános cím, például "A nagy kaland".
        /// </summary>
		public string MovieTitle
		{
			get { return _movieTitle; }
			set { _movieTitle = value; }
		}

        // ToString metódus, amely visszaadja a film adatait egy formázott stringben.
        public override string ToString()
		{
			return $"Movie Title: {_movieTitle}, Movie Length: {_movieLength} minutes, Movie Genre: {_movieGenre}";
        }
    }
}
