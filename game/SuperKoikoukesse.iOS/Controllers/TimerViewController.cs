// Copyright (c) 2013 Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
  /// <summary>
  /// Timer view controller.
  /// </summary>
  public partial class TimerViewController : UIViewController
  {
    #region Members

    private float _startTime = 0f;
    private float _size = 0f;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SuperKoikoukesse.iOS.TimerViewController"/> class.
    /// </summary>
    /// <param name="currentTime">Current time.</param>
    public TimerViewController (float currentTime) : base ("TimerView", null) 
    {
      _startTime = currentTime;
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Position of the current timer
      _size = ViewCurrentTimer.Frame.X;

      // Init label
      SetLabel(_startTime);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Change the timer value.
    /// Invoked on UI thread.
    /// </summary>
    /// <param name="currentTime">Current time.</param>
    /// <param name="totalTime">Total time.</param>
    public void ChangeTime(float currentTime, float totalTime)
    {
      if (currentTime < 0)
        return;

      // Calculate the position
      var position = currentTime * _size / totalTime;

      // Change
      this.InvokeOnMainThread(() => {
        SetPosition(position);
        SetLabel(currentTime);
      });
    }

    /// <summary>
    /// Change the position of the timer.
    /// </summary>
    /// <param name="position">Position.</param>
    private void SetPosition(float position)
    {
      if (position < 0)
        return;

      // Update the frame
      var frame = ViewCurrentTimer.Frame;
      frame.X = position;
      ViewCurrentTimer.Frame = frame;
    }

    /// <summary>
    /// Change the time of the timer label.
    /// </summary>
    /// <param name="time">Time.</param>
    private void SetLabel(float time)
    {
      // Update the label
      LabelCurrentTime.Text = time.ToString("00");
    }

    #endregion

    #region Properties
    #endregion
  }
}

