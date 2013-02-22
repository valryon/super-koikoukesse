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
#endregion

namespace SuperKoikoukesse.Core.Main
{
    /// <summary>
    /// Main game class, initializing all the good stuff.
    /// </summary>
    public class SuperKoikoukesseGame : Game
    {
        private GraphicsDeviceManager m_deviceManager;
        private GameContext m_context;
        private bool m_initializeResolution;
        private FpsCounter counter;

        // Resolution
        // -- Device
        private int GameResolutionWidth, GameResolutionHeight;
        // -- Virtual
        private int DeviceResolutionWidth, DeviceResolutionHeight;

        private GameState m_currentGameState;

        public SuperKoikoukesseGame()
            : base()
        {
            // Basic program parameters
            Window.Title = "SuperKoiKouKesse v0.001";
            Content.RootDirectory = "Content";

            GameResolutionWidth = 1024;
            GameResolutionHeight = 720;

            DeviceResolutionWidth = 1024;
            DeviceResolutionHeight = 720;

            // Force the first resolution set
            m_initializeResolution = true;

            // Graphics manager must be created in constructor
            m_deviceManager = new GraphicsDeviceManager(this);

            // Create game engines and objects
            m_context = new GameContext(this);
            m_context.IsDebugMode = true;

            // FPS counter
            counter = new FpsCounter(m_context);

            // Define quizz as main state for debug
            m_currentGameState = new Quizz(m_context);
        }

        protected override void LoadContent()
        {
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
                m_context.InitializeResolution(this, new Vector2(DeviceResolutionWidth, DeviceResolutionHeight), new Vector2(GameResolutionWidth, GameResolutionHeight), false);

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

        protected override void Draw (GameTime gameTime)
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
    }
}
