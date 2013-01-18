using System.Reflection;
using Glitch.Engine.Core;
using Glitch.Engine.Content;
using Glitch.Engine.Input.Devices;
using Glitch.Engine.Input;
using Glitch.Engine.Util;

namespace SuperKoiKouKesse.Game
{
    [TextureContent(AssetName = "empty", AssetPath = "gfxs/engine/empty", IsEmptyTexture = true)]
    [FontContent(AssetName = "defaultFont", AssetPath = "fonts/defaultFont", IsDefaultFont = true)]
    public class SuperKoiKouKesseGame : GlitchApplication
    {
        FpsCounter counter = new FpsCounter();

        public SuperKoiKouKesseGame()
			: base("Super KoiKouKesse", "Content", "0")
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Inputs
            //-- Register keyboard
            //KeyboardDevice keyboard = new KeyboardDevice(LogicalPlayerIndex.One);
            //keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.Space, MappingButtons.A);
            //keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.LeftControl, MappingButtons.B);
            //keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.RightControl, MappingButtons.X);
            //keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.O, MappingButtons.LB);
            //keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.P, MappingButtons.RB);
            //keyboard.MapLeftThumbstick(Microsoft.Xna.Framework.Input.Keys.Up, Microsoft.Xna.Framework.Input.Keys.Down, Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.Right);

            //InputManager.RegisterDevice(keyboard);

            ////-- Register mouse
            //MouseDevice mouse = new MouseDevice(LogicalPlayerIndex.One);
            //InputManager.RegisterDevice(mouse);

            //// Game states
            //GameStateManager.RegisterGameState(new IngameState());

            //GameStateManager.LoadGameState(GameStateManager.GetGameState<IngameState>());
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
#if DEBUG
            counter.Update(gameTime);
#endif

            base.Update(gameTime);
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
#if DEBUG
            SpriteBatch.BeginNoCamera();
            counter.Draw(SpriteBatch, new Microsoft.Xna.Framework.Vector2(100, 0));
            SpriteBatch.End();
#endif

            base.Draw(gameTime);
        }

        protected override int GameResolutionWidth
        {
            get { return 800; }
        }

        protected override int GameResolutionHeight
        {
            get { return 600; }
        }

        protected override int ScreenResolutionWidth
        {
            get { return 800; }
        }

        protected override int ScreenResolutionHeight
        {
            get { return 600; }
        }

        protected override bool IsFullscreen
        {
            get { return false; }
        }

        protected override Assembly[] GameAssemblies
        {
            get { return new Assembly[] { typeof(SuperKoiKouKesseGame).Assembly, /*typeof(Test.Data.Players.Player).Assembly */}; }
        }
    }
}
