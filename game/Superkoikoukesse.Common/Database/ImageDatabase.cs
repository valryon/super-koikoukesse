using System;
using System.IO;

namespace Superkoikoukesse.Common
{
	public class ImageDatabase
	{
		#region Singleton 
		
		private static ImageDatabase mInstance;
		
		private ImageDatabase ()
		{
		}
		
		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static ImageDatabase Instance {
			get {
				if(mInstance == null) {
					mInstance = new ImageDatabase();
				}
				
				return mInstance;
			}
		}
		
		#endregion

		private string mImagesRootLocation;

		/// <summary>
		/// Initialize the image database service
		/// </summary>
		public void Initialize (string imagesRootLocation) {
			mImagesRootLocation = imagesRootLocation;
		}

		/// <summary>
		/// Get the image of a game.
		/// </summary>
		/// <param name="game">Game.</param>
		public string Getimage(GameEntry game) {
			return Path.Combine (mImagesRootLocation, game.ImagePath);
		}
	}
}

