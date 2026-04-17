using System;
using System.Collections.Generic;
using System.Text;

namespace Beadando_CSharp_26
{
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

        public string MovieGenre
		{
			get { return _movieGenre; }
			set { _movieGenre = value; }
		}

		public int MovieLength
		{
			get { return _movieLength; }
			set { _movieLength = value; }
		}

		public string MovieTitle
		{
			get { return _movieTitle; }
			set { _movieTitle = value; }
		}

        public override string ToString()
		{
			return $"Movie Title: {_movieTitle}, Movie Length: {_movieLength} minutes, Movie Genre: {_movieGenre}";
        }
    }
}
