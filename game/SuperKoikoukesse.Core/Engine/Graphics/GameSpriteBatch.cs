using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Engine.Content;
using SuperKoikoukesse.Core.Engine.Exceptions;

namespace SuperKoikoukesse.Core.Engine.Graphics
{
    public class GameSpriteBatch
    {
        /// <summary>
        /// The XNA spritebatch
        /// </summary>
        private SpriteBatch m_spriteBatch;

        /// <summary>
        /// The content manager
        /// </summary>
        private ContentLoader m_contentLoader;

        /// <summary>
        /// XNA Graphics manager
        /// </summary>
        private GraphicsDeviceManager m_graphics;

        /// <summary>
        /// Camera infos
        /// </summary>
        private Camera2D m_currentCamera;

        /// <summary>
        /// Create a new custom spritebatch using proxy pattern
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="context"></param>
        public GameSpriteBatch(GraphicsDeviceManager graphics, ContentLoader contentLoader)
        {
            m_graphics = graphics;
            m_contentLoader = contentLoader;
            m_spriteBatch = new SpriteBatch(m_graphics.GraphicsDevice);
        }

        #region Start drawing

        /// <summary>
        /// Render without any camera (For HUD only !)
        /// </summary>
        public void BeginNoCamera()
        {
            m_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ResolutionHelper.ResolutionMatrix);
            m_currentCamera = null;
        }

        /// <summary>
        /// Using the given camera, start rendering a scene
        /// </summary>
        public void Begin(Camera2D camera)
        {
            this.Begin(camera, SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        /// <summary>
        /// Using the given camera and parameters, start rendering a scene
        /// </summary>
        /// <param name="spriteSortMode"></param>
        /// <param name="blendState"></param>
        public void Begin(Camera2D camera, SpriteSortMode spriteSortMode, BlendState blendState)
        {
            m_spriteBatch.Begin(spriteSortMode, blendState, SamplerState.PointClamp, null, null, null, camera.CameraMatrix);
            m_currentCamera = camera;
        }

        #endregion

        #region End drawing

        /// <summary>
        /// End drawing
        /// </summary>
        public void End()
        {
            m_spriteBatch.End();

            // Caemra may be drawing things
            if (m_currentCamera != null)
            {
                // Draw the fade in/out
                // TODO Wtf
                m_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ResolutionHelper.ResolutionMatrix);
                m_currentCamera.DrawFade(this);
                m_spriteBatch.End();
            }
        }

        #endregion

        #region Screen cleaning

        /// <summary>
        /// Clear all the drawable zone (= the window)
        /// </summary>
        /// <param name="color"></param>
        public void ClearDevice(Color color)
        {
            m_spriteBatch.GraphicsDevice.Clear(color);
        }

        /// <summary>
        /// Clear only the viewport zone (= in-game)
        /// </summary>
        /// <param name="color"></param>
        public void ClearViewport(Color color)
        {
            m_spriteBatch.Begin();
            DrawRectangle(new Rectangle(0, 0, m_spriteBatch.GraphicsDevice.Viewport.Width, m_spriteBatch.GraphicsDevice.Viewport.Height),
                                    color);
            m_spriteBatch.End();
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draw on screen giving texture name
        /// </summary>
        public void Draw(string textureName, Rectangle dst, Color color, Rectangle? src = null, float rotation = 0f, Vector2 origin = new Vector2(), SpriteEffects flip = SpriteEffects.None)
        {
            Texture2D texture = m_contentLoader.GetTexture(textureName);

            if (texture == null) throw new GameException("Unknow texture: " + textureName);

            Draw(texture, dst, color,src, rotation, origin, flip);
        }

        /// <summary>
        /// Draw on screen
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
        /// Draw text on the screen using default font
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="color"></param>
        public void DrawString(string text, Vector2 location, Color color)
        {
            m_spriteBatch.DrawString(m_contentLoader.Font, text, location, color);
        }

        /// <summary>
        /// Draw text on the screen
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="color"></param>
        public void DrawString(SpriteFont font, string text, Vector2 location, Color color)
        {
            m_spriteBatch.DrawString(font, text, location, color);
        }


        #endregion

        /// <summary>
        /// Access the content manager
        /// </summary>
        public ContentLoader Content
        {
            get
            {
                return m_contentLoader;
            }
        }
    }
}
