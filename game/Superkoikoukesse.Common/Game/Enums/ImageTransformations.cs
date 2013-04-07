using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Available transformation on images
	/// </summary>
	public enum ImageTransformations : int
	{
		None = 0,
		Unzoom = 1,
		Pixelization = 2,
		ProgressiveDrawing = 3
	}
}

