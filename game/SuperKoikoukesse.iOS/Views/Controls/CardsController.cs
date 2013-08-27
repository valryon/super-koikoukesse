// Copyright (c) 2013 Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Drawing;

namespace SuperKoikoukesse.iOS
{
  public class CardsController
  {
    #region Constants

    private const int FIRST_PAGE = 2;

    #endregion

    #region Members

    /// <summary>
    /// The scroll view.
    /// </summary>
    private UIScrollView _scrollView;

    /// <summary>
    /// The page control.
    /// </summary>
    private UIPageControl _pageControl;

    /// <summary>
    /// The cards list.
    /// </summary>
    private List<AbstractCardViewController> _cards;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SuperKoikoukesse.iOS.CardsController"/> class.
    /// </summary>
    /// <param name="scrollView">Scroll view.</param>
    /// <param name="pageControl">Page control.</param>
    public CardsController(UIScrollView scrollView, UIPageControl pageControl)
    {
      _scrollView = scrollView;
      _pageControl = pageControl;

      _cards = new List<AbstractCardViewController>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init()
    {
      // Add events
      _pageControl.ValueChanged += OnValueChanged;
      _scrollView.DecelerationEnded += OnDecelerationEnded;

      int count = _cards.Count;

      SetScrollFrame(count, _scrollView);

      for (int i = 0; i < count; i++)
      {
        // Compute location and size
        var frame = _scrollView.Frame;
        var location = new PointF();

        // Change location
        location.X = frame.Width * i;
        frame.Location = location;

        // Set the frame of the card
        _cards[i].View.Frame = frame;

        // Add the card to the scrollview
        _scrollView.AddSubview(_cards[i].View);
      }

      // Set the page number
      _pageControl.Pages = count;

      // Set 3rd page as the first displayed
      _pageControl.CurrentPage = FIRST_PAGE;

      // Move
      MoveScrollViewToPage(FIRST_PAGE);
    }

    /// <summary>
    /// Add a card to the controller.
    /// </summary>
    /// <param name="card">Card.</param>
    public void AddCard(AbstractCardViewController card)
    {
      _cards.Add(card);
    }

    /// <summary>
    /// Clear all the cards from the controller.
    /// </summary>
    public void Clear()
    {
      _cards.Clear();
    }

    private void SetScrollFrame(int count, UIScrollView scrollView)
    {
      // Set the frame of the scrollView
      var frame = scrollView.Frame;

      frame.Width = frame.Width * count;
      scrollView.ContentSize = frame.Size;
    }

    private void MoveScrollViewToPage(int page)
    {
      // Set the scrollview position
      _scrollView.SetContentOffset(new PointF(page * _scrollView.Frame.Width, 0), true);

      // Change page
      NotifyPageChanged(_pageControl.CurrentPage);
    }

    private void NotifyPageChanged(int page)
    {
      ChangePageControlColor(page);

      // TODO set the current page here
      if (CardChanged != null)
        CardChanged(_cards[page]);
    }

    private void ChangePageControlColor(int page)
    {
      var color = _cards[page].GetColor();
      _pageControl.CurrentPageIndicatorTintColor = color;
    }

    #endregion

    #region Handlers

    private void OnValueChanged(object sender, EventArgs e)
    {
      MoveScrollViewToPage(_pageControl.CurrentPage);
    }

    private void OnDecelerationEnded(object sender, EventArgs e) 
    {
      // Set the new page
      double page = Math.Floor((_scrollView.ContentOffset.X - _scrollView.Frame.Width / 2) / _scrollView.Frame.Width) + 1;
      _pageControl.CurrentPage = (int) page;

      // Notify
      NotifyPageChanged(_pageControl.CurrentPage);
    }

    #endregion

    #region Properties

    public event Action<AbstractCardViewController> CardChanged;

    public int Count
    {
      get 
      {
        return _cards.Count;
      }
    }

    #endregion
  }
}

