// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Avaialable difficulties 
	/// </summary>
	public enum GameDifficulty : int
	{
		EASY = 0,
		NORMAL = 1,
		HARD = 2,
		INSANE = 3
	}

  public static class GameDifficultyHelper
  {
    /// <summary>
    /// Get a difficulty for a string.
    /// </summary>
    /// <returns>
    /// A difficulty for a corresponding string.
    /// If nothing matches, return GameDifficulty.Easy.
    /// </returns>
    /// <param name="difficulty">
    /// A value. The case doesn't matter.
    /// </param>
    public static GameDifficulty Convert(string value)
    {
      value = value.ToLower();

      switch (value)
      {
        case "normal":
          return GameDifficulty.NORMAL;

        case "hard":
          return GameDifficulty.HARD;

        case "insane":
          return GameDifficulty.INSANE;

        default: 
          return GameDifficulty.EASY;
      }
    }
  }
}

