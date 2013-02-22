using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Data
{
    /// <summary>
    /// Question of a quizz
    /// </summary>
    public abstract class Question
    {
        protected GameContext context;

        public Question(GameContext context)
        {
            this.context = context;
            Answers = new List<Answer>();
        }

        /// <summary>
        /// Load content for this question
        /// </summary>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Update the question
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {

        }

        /// <summary>
        /// Draw the question
        /// </summary>
        /// <param name="destination">Where to draw</param>
        public virtual void Draw(Rectangle destination)
        {
        }

        /// <summary>
        /// Available answers
        /// </summary>
        public List<Answer> Answers
        {
            get;
            set;
        }
    }
}
