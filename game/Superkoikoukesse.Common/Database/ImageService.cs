using System;
using System.IO;

namespace Superkoikoukesse.Common
{
	public class ImageService
	{
		#region Singleton 
		
		private static ImageService m_instance;
		
		private ImageService ()
		{
		}
		
		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static ImageService Instance {
			get {
				if(m_instance == null) {
					m_instance = new ImageService();
				}
				
				return m_instance;
			}
		}
		
		#endregion

		private string m_imagesRootLocation;

		/// <summary>
		/// Initialize the image database service
		/// </summary>
		public void Initialize (string imagesRootLocation) {
			m_imagesRootLocation = imagesRootLocation;
		}

		/// <summary>
		/// Get the image of a game.
		/// </summary>
		/// <param name="game">Game.</param>
		public string Getimage(GameInfo game) {
			return Path.Combine (m_imagesRootLocation, game.ImagePath);
		}
	}
}

