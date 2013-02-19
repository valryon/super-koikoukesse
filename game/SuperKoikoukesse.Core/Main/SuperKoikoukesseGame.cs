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
        private GameContext m_context;
        private bool m_changeResolution;

        public SuperKoikoukesseGame()
            : base()
        {
            Content.RootDirectory = "Content";
            m_context = new GameContext(this);

            m_changeResolution = true;
            m_context.WindowSize = new Vector2(1280, 1024);
            m_context.ViewportSize = new Vector2(1024, 720);
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
            if (m_changeResolution)
            {
                m_changeResolution = false;
                m_context.Camera.Initialize(m_context.WindowSize, m_context.ViewportSize);
            }

            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
		{
            m_context.Camera.ClearScreen();
        }
    }
}
