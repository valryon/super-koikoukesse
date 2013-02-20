using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Engine.GameStates
{
    /// <summary>
    /// Defining a state for our simple state machine
    /// </summary>
    public abstract class GameState
    {
        /// <summary>
        /// The game context to access asset manager, camera, etc
        /// </summary>
        protected GameContext context;

        /// <summary>
        /// Create a new game state
        /// </summary>
        /// <param name="context"></param>
        public GameState(GameContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Load all the content here
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Trash useless assets
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Update the state
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw things on the screen because after all we're in a fucking video game
        /// </summary>
        public abstract void Draw();
    }
}
