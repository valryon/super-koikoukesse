using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine.Graphics;
using SuperKoikoukesse.Core.Engine;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Engine.Content;
using SuperKoikoukesse.Core.Engine.Tools;
using SuperKoikoukesse.Core.Main;

namespace SuperKoikoukesse.Core.Engine
{
    public class GameContext
    {
        public GameContext(SuperKoikoukesseGame game)
        {
            // Create every piece of the engine
            // -- Graphics
            ContentLoader contentLoader = new ContentLoader(game.Content, game.GraphicsDeviceManager.GraphicsDevice);
            SpriteBatch = new GameSpriteBatch( game.SpriteBatch, contentLoader);

            // -- Tools
            Random = new RandomHelper(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Initialize the resolution (window + game)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="windowSize"></param>
        /// <param name="viewportSize"></param>
        /// <param name="fullscreen"></param>
        public void InitializeResolution(SuperKoikoukesseGame game, Vector2 windowSize, Vector2 viewportSize, bool fullscreen)
        {
            // Set resolution
            WindowSize = new Rectangle(0, 0, (int)windowSize.X, (int)windowSize.Y);
            ViewportSize = new Rectangle(0, 0, (int)viewportSize.X, (int)viewportSize.Y);

            ResolutionHelper.Initialize(game.GraphicsDeviceManager, WindowSize.Width, WindowSize.Height, ViewportSize.Width, ViewportSize.Height, fullscreen);
        }

        /// <summary>
        /// Window
        /// </summary>
        public Rectangle WindowSize { get; private set; }

        /// <summary>
        /// Virtual game window
        /// </summary>
        public Rectangle ViewportSize { get; private set; }

        /// <summary>
        /// Display or hide debug infos
        /// </summary>
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Sprite batch
        /// </summary>
        public GameSpriteBatch SpriteBatch {get; private set;}

        /// <summary>
        /// Shortcut to the content loader
        /// </summary>
        public ContentLoader Content
        {
            get
            {
                return SpriteBatch.Content;
            }
        }

        /// <summary>
        /// Random numbers utilities
        /// </summary>
        public RandomHelper Random { get; private set; }
    }
}
