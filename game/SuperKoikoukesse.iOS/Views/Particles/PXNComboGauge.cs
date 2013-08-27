// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.CoreAnimation;
using System.Drawing;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
  /// <summary>
  /// PXN combo gauge particles emitter layer.
  /// </summary>
  public class PXNComboGauge : CAEmitterLayer
  {
    #region Members
    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SuperKoikoukesse.iOS.PXNComboGauge"/> class.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public PXNComboGauge(float x, float y, float width, float height)
    {
      // Emitter properties
      Position = new PointF(x, y);
      ZPosition = 0;
      Size = new SizeF(width, height);
      Depth = 0.00f;
      Shape = CAEmitterLayer.ShapeRectangle;
      RenderMode = CAEmitterLayer.RenderAdditive;
      Seed = 1813220680;

      // Cell properties
      var cellOne = CreateCellOne();

      Cells = new CAEmitterCell[] {cellOne};
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create a cell.
    /// </summary>
    /// <returns>The cell one.</returns>
    private CAEmitterCell CreateCellOne()
    {
      var cell = new CAEmitterCell();

      cell.Name = "one";
      cell.Enabled = true;

      cell.Contents = new UIImage("particle.png").CGImage;
      cell.ContentsRect = new RectangleF(0.00f, 0.00f, 1.00f, 1.00f);

      cell.MagnificationFilter = CALayer.FilterLinear;
      cell.MinificationFilter = CALayer.FilterLinear;
      cell.MinificationFilterBias = 0.00f;

      cell.Scale = 0;
      cell.ScaleRange = 0.50f;
      cell.ScaleSpeed = 0.10f;

      cell.Color = UIColor.FromRGBA(0.16f, 0.57f, 0.94f, 1f).CGColor;
      cell.RedRange = 0.70f;
      cell.GreenRange = 0.00f;
      cell.BlueRange = 0.00f;
      cell.AlphaRange = 1.00f;

      cell.RedSpeed = 0.00f;
      cell.GreenSpeed = 0.00f;
      cell.BlueSpeed = 0.00f;
      cell.AlphaSpeed = -1.00f;

      cell.LifeTime = 3f;
      cell.LifetimeRange = 0.80f;
      cell.BirthRate = 300;
      cell.Velocity = 0.00f;
      cell.VelocityRange = 10.00f;
      cell.AccelerationX = -20.00f;
      cell.AccelerationY = 0.00f;
      cell.AccelerationZ = 10.00f;

      cell.Spin = 0.000f;
      cell.SpinRange = 0.000f;
      cell.EmissionLatitude = 0.000f;
      cell.EmissionLongitude = 0.000f;
      cell.EmissionRange = 6.283f;

      return cell;
    }

    #endregion

    #region Properties
    #endregion
  }
}

