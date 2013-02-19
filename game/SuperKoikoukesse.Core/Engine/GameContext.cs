using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine.Graphics;
using SuperKoikoukesse.Core.Engine;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Engine.Content;

namespace SuperKoikoukesse.Core.Engine
{
    public class GameContext
    {
        private GameContext m_instance;

        // Engines instances
        private ContentLoader m_contentLoader;
        private Camera m_camera;

        public GameContext(Game game)
        {
            m_instance = this;

            GraphicsDeviceManager deviceManager = new GraphicsDeviceManager(game);

            m_contentLoader = new ContentLoader(game.Content, deviceManager.GraphicsDevice);
            m_camera = new Camera(deviceManager, m_contentLoader);
            
        }

        /// <summary>
        /// Window
        /// </summary>
        public Vector2 WindowSize { get; set; }

        /// <summary>
        /// Virtual game window
        /// </summary>
        public Vector2 ViewportSize { get; set; }

        /// <summary>
        /// Current game camera
        /// </summary>
        public Camera Camera
        {
            get
            {
                return m_camera;
            }
        }

        /// <summary>
        /// Game content loader
        /// </summary>
        public ContentLoader ContentLoader
        {
            get
            {
                return m_contentLoader;
            }
        }
    }
}
