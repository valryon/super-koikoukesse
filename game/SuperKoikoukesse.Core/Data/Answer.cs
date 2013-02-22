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
            context.SpriteBatch.DrawString(Text, location, Color.White);
        }
    }
}
