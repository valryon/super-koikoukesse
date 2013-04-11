using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Filter for game database
	/// </summary>
	public class Filter
	{
		/// <summary>
		/// Minimal year for a game
		/// </summary>
		/// <value>The minimum year.</value>
		public int MinYear { get; private set; }

		/// <summary>
		/// Maximal year for a game
		/// </summary>
		/// <value>The max year.</value>
		public int MaxYear { get; private set; }

		/// <summary>
		/// Specify pbulishers
		/// </summary>
		/// <value>The publisher.</value>
		public List<string> Publishers { get; private set; }

		/// <summary>
		/// Specify a genre
		/// </summary>
		/// <value>The genre.</value>
		public List<string> Genres { get; private set; }

		/// <summary>
		/// Specify a platform
		/// </summary>
		/// <value>The platform.</value>
		public List<string> Platforms { get; private set; }

		private Random random;
		private List<GameInfo> matchingGames;

		public Filter (int minYear = 0, int maxYear = 9999, List<string> publishers = null, List<string> genres = null, List<string> platforms = null)
		{
			MinYear = minYear;
			MaxYear = maxYear;

			Publishers = publishers;
			Genres = genres;
			Platforms = platforms;

			random = new Random (DateTime.Now.Millisecond);
		}

		/// <summary>
		/// Caching database
		/// </summary>
		/// <param name="loadingComplete">Loading complete for n games.</param>
		public void Load (Action<int> loadingComplete)
		{
			BackgroundWorker worker = new BackgroundWorker ();

			worker.DoWork += (object sender, DoWorkEventArgs e) => {

				matchingGames = DatabaseService.Instance.ReadGames (MinYear, MaxYear, Publishers, Genres, Platforms);

				loadingComplete (matchingGames.Count);

			};

			worker.RunWorkerAsync ();
		}

		/// <summary>
		/// Get a game corresponding to this filter
		/// </summary>
		/// <returns>The games.</returns>
		/// <param name="count">Count.</param>
		public GameInfo GetGame ()
		{
			// Random game
			int randomIndex = random.Next (matchingGames.Count);

			return matchingGames [randomIndex];
		}

		/// <summary>
		/// Load a predefined filter for Stunfest
		/// </summary>
		public void StunfestMode ()
		{
			if(Genres != null) {
				Genres.Clear();
			}
			else {
				Genres = new List<string>();
			}
			Genres.Add ("combat");
		}
	}
}

