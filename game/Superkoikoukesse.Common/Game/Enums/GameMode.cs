// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Available game modes
	/// </summary>
	public enum GameMode : int
	{
		SCORE = 0,
		TIME = 1,
		SURVIVAL = 2,
		VERSUS = 3
	}

  public static class GameModeHelper
  {
    /// <summary>
    /// Get a mode for a string.
    /// </summary>
    /// <returns>
    /// A mode for a corresponding string.
    /// If nothing matches, return GameMode.Score.
    /// </returns>
    /// <param name="value">
    /// A string. The case doesn't matter.
    /// </param>
    public static GameMode Convert(string value)
    {
      value = value.ToLower();

      switch (value)
      {
        case "time":
          return GameMode.TIME;

        case "survival":
          return GameMode.SURVIVAL;

        case "versus":
          return GameMode.VERSUS;

        default: 
          return GameMode.SCORE;
      }
    }

    /// <summary>
    /// Get a mode for an int.
    /// </summary>
    /// <returns>
    /// A mode for a corresponding integer.
    /// </returns>
    /// <param name="value">
    /// An integer.
    /// </param>
    public static GameMode Convert(int value)
    {
      switch (value)
      {
        case 0:
          return GameMode.SCORE;
        case 1:
          return GameMode.TIME;
        case 2:
          return GameMode.SURVIVAL;
        case 3: 
          return GameMode.VERSUS;

        default:
          return GameMode.SCORE;
      }
    }
  }
}

