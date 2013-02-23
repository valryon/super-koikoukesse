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
using SuperKoikoukesse.Core.Engine.GameStates;
using SuperKoikoukesse.Core.Engine.States;
using SuperKoikoukesse.Core.Engine.Tools;
using SuperKoikoukesse.Core.Data;
#endregion

namespace SuperKoikoukesse.Core.Main
{
    /// <summary>
    /// Main game class, initializing all the good stuff.
    /// </summary>
    public class SuperKoikoukesseGame : Game
    {
        private GraphicsDeviceManager m_deviceManager;
		private SpriteBatch m_spriteBatch;
		private GameContext m_context;
        private bool m_initializeResolution;
        private FpsCounter counter;

        private GameState m_currentGameState;

        public SuperKoikoukesseGame()
            : base()
        {
			// Graphics manager & sprite batch must be created directly in constructor
			m_deviceManager = new GraphicsDeviceManager(this);

            // Basic program parameters
            Window.Title = "SuperKoiKouKesse v0.001";
			IsMouseVisible = true;
            Content.RootDirectory = "Content";

            // Force the first resolution set
            m_initializeResolution = true;
        }

        protected override void LoadContent()
        {
			// Create a spritebatch only here
			// Otherwise GraphicDevice isn't initialized
			m_spriteBatch = new SpriteBatch(m_deviceManager.GraphicsDevice);

			// Create game engines and objects
			// !!!! Content loader user graphics device wich is initialized only here!
			m_context = new GameContext(this);

			// Ok on MAC OS X we have to set the resolution here... but not on Windows!
#if OSX
			m_context.InitializeResolution(this, new Vector2(Constants.DeviceResolutionWidth, Constants.DeviceResolutionHeight), new Vector2(Constants.GameResolutionWidth, Constants.GameResolutionHeight), false);
#endif
			m_context.IsDebugMode = true;
			
			// FPS counter
			counter = new FpsCounter(m_context);
			
			// Define quizz as main state for debug
			m_currentGameState = new Quizz(m_context);

            // Load fonts, common assets, etc
            m_context.Content.LoadInitialContent();

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
            if (m_initializeResolution)
            {
                m_initializeResolution = false;
                m_context.InitializeResolution(this, new Vector2(Constants.DeviceResolutionWidth, Constants.DeviceResolutionHeight), new Vector2(Constants.GameResolutionWidth, Constants.GameResolutionHeight), false);

                // Reset camera for a good location
                if (m_currentGameState != null)
                {
                    m_currentGameState.Camera.Reset();
                }
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

        protected override void Draw(GameTime gameTime)
        {
            // Clean the screen
            m_context.SpriteBatch.ClearDevice(Color.Black);
            m_context.SpriteBatch.ClearViewport(Color.CornflowerBlue);

            // -- The current state
            m_currentGameState.Draw();

            // Debug mode
            if (m_context.IsDebugMode)
            {
                // Display FPS count
                m_context.SpriteBatch.BeginNoCamera();
                counter.Draw(Vector2.One);
                m_context.SpriteBatch.End();
            }
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                return m_deviceManager;
            }
        }

		public SpriteBatch SpriteBatch {
			get {
				return m_spriteBatch;
			}
		}
    }
}
