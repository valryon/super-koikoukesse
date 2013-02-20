using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine.GameStates;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Engine.States
{
    /// <summary>
    /// The quizz game state
    /// </summary>
    public class Quizz : GameState
    {
        private string backgroundImageAsset = "gfxs/backgrounds/quizz_background";

        public Quizz(GameContext context)
            : base(context)
        {

        }

        public override void LoadContent()
        {
            // Load background
            context.ContentLoader.LoadTexture(backgroundImageAsset);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            // Draw the background
            context.Camera.Draw(context.ContentLoader.GetTexture(backgroundImageAsset), context.ViewportSize, Color.White);
        }
    }
}
