// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using Superkoikoukesse.Common;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
  /// <summary>
  /// Simplyfing the process to launch a new game
  /// </summary>
  public class GameLauncher
  {
    public GameMode SelectedMode { get; set; }

    public GameDifficulties SelectedDifficulty{ get; set; }

    public Filter SelectedFilter{ get; set; }

    public string SegueId{ get; set; }

    private UIViewController mController;

    public GameLauncher(UIViewController c)
    {
      mController = c;
    }

    public void Launch(string segueId, GameMode mode, GameDifficulties difficulty, Filter filter)
    {
      SegueId = segueId;
      SelectedMode = mode;
      SelectedDifficulty = difficulty;
      SelectedFilter = filter;

      if (SelectedFilter == null)
      {
        SelectedFilter = new Filter("0", "Siphon filter", "defaultIcon");
      }

      UIViewController.InvokeInBackground(() => {
        int gamesCount = SelectedFilter.Load();

        mController.BeginInvokeOnMainThread(() => {
          if (gamesCount < 30)
          {
            Dialogs.ShowDebugFilterTooRestrictive();
          }
          else
          {
            mController.PerformSegue(SegueId, mController);
          }
        });
      });
    }

    public void Prepare(GameViewController gameVc)
    {
      // Prepare quizz
      gameVc.InitializeQuizz(SelectedMode, SelectedDifficulty, SelectedFilter);
    }
  }
}

