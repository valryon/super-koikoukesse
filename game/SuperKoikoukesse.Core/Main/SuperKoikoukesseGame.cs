#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using SuperKoikoukesse.Core.Engine;
using SuperKoikoukesse.Core.Engine.Graphics;
#endregion

namespace SuperKoikoukesse.Core.Main
{
    /// <summary>
    /// Main game class, initializing all the good stuff.
    /// </summary>
    public class SuperKoikoukesseGame : Game
    {
        /// <summary>
        /// Game is a singleton
        /// </summary>
        private static SuperKoikoukesseGame m_instance;

        // Engines instances

        private ContentLoader m_contentLoader;
        private Camera m_camera;

        public SuperKoikoukesseGame()
            : base()
        {
            m_instance = this;

            Content.RootDirectory = "Content";
            m_contentLoader = new ContentLoader(Content);
            m_camera = new Camera(new GraphicsDeviceManager(this));
            
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
		{
            m_camera.ClearScreen();
        }

        /// <summary>
        /// Current game camera
        /// </summary>
        public static Camera Camera
        {
            get
            {
                return m_instance.m_camera;
            }
        }

        /// <summary>
        /// Game content loader
        /// </summary>
        public static ContentLoader ContentLoader
        {
            get
            {
                return m_instance.m_contentLoader;
            }
        }
    }
}
