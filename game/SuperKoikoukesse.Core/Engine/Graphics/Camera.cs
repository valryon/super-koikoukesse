using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperKoikoukesse.Core.Main;
using SuperKoikoukesse.Core.Engine.Content;

namespace SuperKoikoukesse.Core.Engine.Graphics
{
    /// <summary>
    /// Manage resolution, camera, sprite drawing... all the hard things.
    /// </summary>
    public class Camera
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;
        private ContentLoader m_contentLoader;

        public Camera(GraphicsDeviceManager graphics, ContentLoader contentLoader)
        {
            m_graphics = graphics;
            m_contentLoader = contentLoader;
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
            DrawRectangle(new Rectangle(0, 0, m_graphics.GraphicsDevice.Viewport.Width, m_graphics.GraphicsDevice.Viewport.Height), viewportColor);
            End();
        }

        /// <summary>
        /// Start drawing on screen
        /// </summary>
        public void Begin()
        {
            m_spriteBatch.Begin();
        }

        /// <summary>
        /// Add something to be draw on "End" call
        /// </summary>
        public void Draw(Texture2D texture, Rectangle dst, Color color, Rectangle? src = null, float rotation = 0f, Vector2 origin = new Vector2(), SpriteEffects flip = SpriteEffects.None)
        {
            m_spriteBatch.Draw(texture, dst, src, color, rotation, origin, flip, 1.0f);
        }

        /// <summary>
        /// Draw a rectangle on the screen
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="color"></param>
        public void DrawRectangle(Rectangle dst, Color color)
        {
            m_spriteBatch.Draw(m_contentLoader.BlankTexture, dst, color);
        }

        /// <summary>
        /// Draw text on the screen
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="location"></param>
        /// <param name="color"></param>
        public void DrawString(SpriteFont font, string text, Vector2 location, Color color)
        {
            m_spriteBatch.DrawString(font, text, location, color);
        }

        /// <summary>
        /// End drawing on screen
        /// </summary>
        public void End()
        {
            m_spriteBatch.End();
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
