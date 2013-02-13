#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SuperKoikoukesse.Core.Main
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SuperKoikoukesseGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D basePlayer;

        public SuperKoikoukesseGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

#if WINDOWS
            basePlayer = Content.Load<Texture2D>(@"gfxs/sprites/player");
#elif MAC
			// Load a Texture2D from a file
			System.IO.Stream stream = TitleContainer.OpenStream("MonoBundle/Content/gfxs/sprites/player.png");
			basePlayer = Texture2D.FromStream(GraphicsDevice, stream);
			
			// Load a SoundEffect from a file 
			// (NOTE: This is NOT in the current version of MonoGame)
			//stream = TitleContainer.OpenStream("Content/My Sound.wav");
			//mySound = SoundEffect.FromStream(stream);
#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			if (basePlayer != null) {
				spriteBatch.Draw (basePlayer, new Rectangle (0, 0, 128, 128), Color.White);
			}
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
