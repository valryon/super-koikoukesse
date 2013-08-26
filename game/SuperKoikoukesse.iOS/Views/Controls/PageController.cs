// Copyright (c) 2013 Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
  public class PageController
  {
    #region Members

    private List<UIViewController> _viewControllers;

    #endregion

    #region Constructors

    public PageController()
    {
      _viewControllers = new List<UIViewController>();
    }

    #endregion

    #region Methods

    public void AddPage(UIViewController viewController)
    {

    }

    #endregion

    #region Properties

    public event Action PageChanged;

    #endregion
  }
}

