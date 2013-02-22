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
            context.Content.LoadTexture(backgroundImageAsset);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        public override void Draw()
        {
            context.SpriteBatch.Begin(Camera);

            // Draw the background
            context.SpriteBatch.Draw(backgroundImageAsset, context.ViewportSize, Color.White);

            context.SpriteBatch.End();
        }
    }
}
