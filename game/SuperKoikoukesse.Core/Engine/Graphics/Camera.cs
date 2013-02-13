using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperKoikoukesse.Core.Engine.Graphics
{
    /// <summary>
    /// Manage resolution, camera, sprite drawing... all the hard things.
    /// </summary>
    public class Camera
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        public Camera(GraphicsDeviceManager graphics)
        {
            m_graphics = graphics;
            m_spriteBatch = new SpriteBatch(m_graphics.GraphicsDevice);
        }

        /// <summary>
        /// Initialize the camera with the given game and window resolution
        /// </summary>
        /// <param name="windowDimension">Resolution of the window / device</param>
        /// <param name="gameDimension">Resolution of the game (virtual)</param>
        public void Initialize(Vector2 windowDimension, Vector2 gameDimension, bool fullscreen = false)
        {
            ResolutionHelper.Initialize(m_graphics, (int)windowDimension.X, (int)windowDimension.Y, (int)gameDimension.X, (int)gameDimension.Y, fullscreen);
        }

        /// <summary>
        /// Clear the screen with default colors
        /// </summary>
        public void ClearScreen()
        {
            ClearScreen(Color.Black, Color.CornflowerBlue);
        }

        /// <summary>
        /// Clear the screen
        /// </summary>
        public void ClearScreen(Color windowColor, Color viewportColor)
        {
            // Clear the whole window
            m_graphics.GraphicsDevice.Clear(windowColor);

            // Clear the virtual resolution
            Begin();
            //DrawRectangle(new Rectangle(0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height),
            //                        color);
            End();
        }

        /// <summary>
        /// Start drawing on screen
        /// </summary>
        public void Begin()
        {

        }

        /// <summary>
        /// Add something to be draw on "End" call
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// End drawing on screen
        /// </summary>
        public void End()
        {
        }

        /// <summary>
        /// Get the current Graphics device
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return m_graphics.GraphicsDevice;
            }
        }
    }
}
