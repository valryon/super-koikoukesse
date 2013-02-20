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
        /// Load new textures
        /// </summary>
        /// <param name="backgroundImageAsset"></param>
        public void LoadTexture(string backgroundImageAsset)
        {
            Texture2D texture = m_contentManager.Load<Texture2D>(backgroundImageAsset);
            m_textures.Add(backgroundImageAsset, texture);
        }

        /// <summary>
        /// Get a known texture
        /// </summary>
        public Texture2D GetTexture(string name)
        {
            return m_textures[name];
        }

        /// <summary>
        /// Useful empty texture to draw non-sprite colored things
        /// </summary>
        public Texture2D BlankTexture { get; private set; }

        public SpriteFont Font { get; private set; }
    }
}
