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
using SuperKoikoukesse.Core.Engine.Bench;
using SuperKoikoukesse.Core.Engine.GameStates;
using SuperKoikoukesse.Core.Engine.States;
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
        private FpsCounter counter;

        private GameState m_currentGameState;

        public SuperKoikoukesseGame()
            : base()
        {
            Window.Title = "SuperKoiKouKesse v0.001";

            Content.RootDirectory = "Content";
            m_context = new GameContext(this);

            m_changeResolution = true;
            m_context.WindowSize = new Rectangle(0,0,1024, 768);
            m_context.ViewportSize = new Rectangle(0, 0, 960, 640);

            m_context.IsDebugMode = true;

            counter = new FpsCounter(m_context);

            // Define quizz as main state for debug
            m_currentGameState = new Quizz(m_context);
        }

        protected override void LoadContent()
        {
            // Load fonts, common assets, etc
            m_context.ContentLoader.LoadInitialContent();

            // Load gamestate specific assets
            m_currentGameState.LoadContent();
        }

        protected override void UnloadContent()
        {
            // Make some space
            if (m_currentGameState != null)
            {
                m_currentGameState.UnloadContent();
            }
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Resolution must be changed in Update to work (MonoGame issue)
            if (m_changeResolution)
            {
                m_changeResolution = false;
                m_context.Camera.Initialize(new Vector2(m_context.WindowSize.Width, m_context.WindowSize.Height), new Vector2(m_context.ViewportSize.Width, m_context.ViewportSize.Height));
            }

            // Update game state
            m_currentGameState.Update(gameTime);

            // Debug mode
            if (m_context.IsDebugMode)
            {
                // Update FPS count
                counter.Update(gameTime);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
		{
            // Clean the screen
            m_context.Camera.ClearScreen();

            // Display batching
            m_context.Camera.Begin();

            // -- The current state
            m_currentGameState.Draw();

            m_context.Camera.End();

            // Debug mode
            if (m_context.IsDebugMode)
            {
                // Display FPS count
                m_context.Camera.Begin();
                counter.Draw(Vector2.One);
                m_context.Camera.End();
            }
        }
    }
}
