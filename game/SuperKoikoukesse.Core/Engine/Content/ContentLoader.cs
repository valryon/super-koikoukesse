using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Main;

namespace SuperKoikoukesse.Core.Engine.Content
{
    /// <summary>
    /// Assets management
    /// </summary>
    public class ContentLoader
    {
        private ContentManager m_contentManager;

        private Dictionary<string, Texture2D> m_textures;

        public ContentLoader(ContentManager content, GraphicsDevice device)
        {
            m_contentManager = content;

            // Create the empty texture
            BlankTexture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Color[] color = new Color[1] { Color.White };
            BlankTexture.SetData(color);

            m_textures = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Load the required content
        /// </summary>
        public void LoadInitialContent()
        {
            Font = m_contentManager.Load<SpriteFont>("fonts/font");
        }

        /// <summary>
        /// Load texture
        /// </summary>
        /// <param name="imageAssetName"></param>
        public void LoadTexture(string imageAssetName)
        {
            try
            {
                Texture2D texture = m_contentManager.Load<Texture2D>(imageAssetName);
                m_textures.Add(imageAssetName, texture);
            }
            catch (Exception e)
            {
                throw new ContentLoadException(imageAssetName + " wasn't loaded.", e);
            }
        }

        /// <summary>
        /// Load texture from file
        /// </summary>
        /// <param name="imageAssetName"></param>
        public void LoadTexture(string imageAssetName, string imageFilePath)
        {
            // TODO From file
            try
            {
                Texture2D texture = m_contentManager.Load<Texture2D>(imageFilePath);
                m_textures.Add(imageAssetName, texture);
            }
            catch (Exception e)
            {
                throw new ContentLoadException(imageAssetName + " (" + imageFilePath + ") wasn't loaded.", e);
            }
        }

        /// <summary>
        /// Get a known texture
        /// </summary>
        public Texture2D GetTexture(string name)
        {
            if (m_textures.ContainsKey(name))
            {
                return m_textures[name];
            }

            return null;
        }

        /// <summary>
        /// Useful empty texture to draw non-sprite colored things
        /// </summary>
        public Texture2D BlankTexture { get; private set; }

        public SpriteFont Font { get; private set; }
    }
}
