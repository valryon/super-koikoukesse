using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Available transformation on images
	/// </summary>
	public enum ImageTransformations : int
	{
		NONE = 0,
		UNZOOM = 1,
		PIXELIZATION = 2,
		PROGRESSIVE_DRAWING = 3,
		TEST = 4
	}
}

