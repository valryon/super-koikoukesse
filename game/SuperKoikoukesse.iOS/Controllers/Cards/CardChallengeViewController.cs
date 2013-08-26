using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class CardChallengeViewController : AbstractCardViewController
  {
    #region Members

    private GameMode _mode;

    #endregion

    #region Constructors

    public CardChallengeViewController() : base("CardChallengeView")
    {

    }

    #endregion

    #region Methods

    public void SetMode(GameMode m)
    {
      _mode = m;
    }

    #endregion

    #region Properties

    public event Action<GameMode, GameDifficulties> DifficultySelected;
    public event Action BackSelected;

    #endregion
  }
}

