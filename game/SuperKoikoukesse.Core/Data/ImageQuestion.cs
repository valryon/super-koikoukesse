using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Data
{
    /// <summary>
    /// Image question
    /// </summary>
    public class ImageQuestion : Question
    {
        /// <summary>
        /// Access to the asset on the db/file system
        /// </summary>
        public string ImagePath {get;set;}

        /// <summary>
        /// The name in the content manager
        /// </summary>
        public string ImageAssetName { get; set; }

        public ImageQuestion(GameContext context)
            : base(context)
        {
        }

        public override void LoadContent()
        {
            // Already loaded?
            if (context.Content.GetTexture(ImageAssetName) == null)
            {
                context.Content.LoadTexture(ImageAssetName, ImagePath);
            }

            base.LoadContent();
        }

        /// <summary>
        /// Update effects on the image
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO Effects
        }

        /// <summary>
        /// Draw the image
        /// </summary>
        public override void Draw(Rectangle destination)
        {
            base.Draw(destination);

            context.SpriteBatch.Draw(ImageAssetName, destination, Color.White);
        }
    }
}
