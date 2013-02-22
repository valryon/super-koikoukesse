using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Engine;

namespace SuperKoikoukesse.Core.Data
{
    /// <summary>
    /// Quizz answer
    /// </summary>
    public class Answer
    {
        protected GameContext context;

        /// <summary>
        /// Full text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is this the right answer?
        /// </summary>
        public bool IsSolution { get; set; }

        public Answer(GameContext context)
        {
            this.context = context;
        }

        public Answer(GameContext context, string text, bool isSolution)
            : this(context)
        {
            this.Text = text;
            this.IsSolution = isSolution;
        }

        /// <summary>
        /// Load content for this answer
        /// </summary>
        public void LoadContent()
        {
            if (context.Content.GetTexture(Constants.QuizzButtonAsset) == null)
            {
                context.Content.LoadTexture(Constants.QuizzButtonAsset);
            }
        }

        /// <summary>
        /// Update the answer
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draw the answer
        /// </summary>
        /// <param name="destination">Where to draw</param>
        public virtual void Draw(Vector2 location)
        {
            Color color = Color.White;

            if (context.IsDebugMode)
            {
                color = (IsSolution ? Color.Green : Color.Red);
            }

            // Draw the frame
            context.SpriteBatch.Draw(Constants.QuizzButtonAsset, new Rectangle((int)location.X, (int)location.Y, Constants.QuizzButtonWidth, Constants.QuizzButtonHeight), color);

            // Write the text
            context.SpriteBatch.DrawString(Text, location + new Vector2(15,10), Color.White);
        }
    }
}
