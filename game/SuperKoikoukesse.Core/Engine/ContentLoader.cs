using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Main;

namespace SuperKoikoukesse.Core.Engine
{
    /// <summary>
    /// Assets management
    /// </summary>
    public class ContentLoader
    {
        private ContentManager m_contentManager;

        /// <summary>
        /// Useful empty texture to draw non-sprite colored things
        /// </summary>
        private Texture2D m_blankTexture;

        public ContentLoader(ContentManager content)
        {
            m_contentManager = content;

            // Create the empty texture
            m_blankTexture = new Texture2D(SuperKoikoukesseGame.Camera.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] color = new Color[1] { Color.White};
            m_blankTexture.SetData(color);
        }
    }
}
