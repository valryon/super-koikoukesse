using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace SuperKoikoukesse.iOS
{
	/// <summary>
	/// Utilities method to create view controls
	/// </summary>
	public static class ViewTools
	{
		public static UIView CreateUIView() 
		{
			// Landscape mode
			UIView view = new UIView(new RectangleF(0,0,UIScreen.MainScreen.Bounds.Height,UIScreen.MainScreen.Bounds.Width));
			view.BackgroundColor = ColorConstants.ToUIColor (ColorConstants.BRAND_WHITE);

			return view;
		}

		public static UILabel Createlabel(RectangleF frame, string text) 
		{
			UILabel label = new UILabel (frame);
			label.TextColor = ColorConstants.ToUIColor (ColorConstants.TEXT_BLACK);
			label.Text = text;
			label.BackgroundColor = new UIColor (0f,0f,0f,0f);

			return label;
		}

		public static UIButton CreateButton(RectangleF frame, string text, Action onClick)
		{
			UIButton button = new UIButton (frame);

			button.SetTitle (text, UIControlState.Normal);
			button.SetTitleColor(ColorConstants.ToUIColor (ColorConstants.TEXT_WHITE), UIControlState.Normal);
			button.SetBackgroundImage(new UIImage("button_small.png"), UIControlState.Normal);

			if (onClick != null) {
				button.TouchDown += (object sender, EventArgs e) => {
					onClick();
				};
			}

			return button;
		}

		public static UIButton CreateImportantButton(RectangleF frame, string text, Action onClick)
		{
			UIButton button = CreateButton (frame, text, onClick);
			button.SetBackgroundImage(new UIImage("button_small_important.png"), UIControlState.Normal);

			return button;
		}
	}
}

