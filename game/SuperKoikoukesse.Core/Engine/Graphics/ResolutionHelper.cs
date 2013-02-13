using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperKoikoukesse.Core.Engine.Graphics
{
    /// <summary>
    /// "Easy" resolution management
    /// </summary>
    /// <remarks>Author : David Gouveia</remarks>
    public static class ResolutionHelper
    {
        private static GraphicsDeviceManager m_graphics;
        private static Matrix m_resolutionMatrix = Matrix.Identity;
        private static bool m_isFullScreen;
        private static bool m_dirty = true;

        /// <summary>
        /// Real resolution width
        /// </summary>
        public static int DeviceWidth { get; private set; }

        /// <summary>
        /// Real resolution height
        /// </summary>
        public static int DeviceHeight { get; private set; }

        /// <summary>
        /// Displayed resolution width
        /// </summary>
        public static int VirtualWidth { get; private set; }

        /// <summary>
        /// Displayed resolution height
        /// </summary>
        public static int VirtualHeight { get; private set; }

        /// <summary>
        /// Viewport X position
        /// </summary>
        public static int ViewportX { get; private set; }

        /// <summary>
        /// Viewport Y position
        /// </summary>
        public static int ViewportY { get; private set; }

        /// <summary>
        /// Viewport Width
        /// </summary>
        public static int ViewportWidth { get; private set; }

        /// <summary>
        /// Viewport Height
        /// </summary>
        public static int ViewportHeight { get; private set; }


        public static Rectangle ViewportDimensionsRect { get; private set; }
        public static Rectangle VirtualDimensionsRect { get; private set; }

        /// <summary>
        /// Transformation Matrix to be resolution independant
        /// </summary>
        public static Matrix ResolutionMatrix
        {
            get
            {
                if (m_dirty)
                {
                    Invalidate();
                    m_dirty = false;
                }

                return m_resolutionMatrix;
            }
        }

        /// <summary>
        /// Intialize the resolution independance
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="deviceWidth"></param>
        /// <param name="deviceHeight"></param>
        /// <param name="virtualWidth"></param>
        /// <param name="virtualHeight"></param>
        /// <param name="fullscreen"></param>
        public static void Initialize(GraphicsDeviceManager graphics, int deviceWidth, int deviceHeight, int virtualWidth, int virtualHeight, bool fullscreen)
        {
            m_graphics = graphics;

            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            SetWindowResolution(deviceWidth, deviceHeight, fullscreen);

            ViewportDimensionsRect = new Rectangle(ViewportX, ViewportY, ViewportWidth, ViewportHeight);
            VirtualDimensionsRect = new Rectangle(0, 0, VirtualWidth, VirtualHeight);
        }

        /// <summary>
        /// Change the resolution of the windows after initialization
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fullscreen"></param>
        public static void SetWindowResolution(int width, int height, bool fullscreen)
        {
            DeviceWidth = width;
            DeviceHeight = height;

            m_isFullScreen = fullscreen;

            ApplyChanges();
            CalculateViewport();

            m_dirty = true;
        }

        /// <summary>
        /// Change the resolution of the game (viewport) after initialization
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetGameResolution(int width, int height)
        {
            VirtualWidth = width;
            VirtualHeight = height;

            ApplyChanges();
            CalculateViewport();

            m_dirty = true;
        }

        private static void Invalidate()
        {
            m_resolutionMatrix = Matrix.CreateScale(
                (float)m_graphics.GraphicsDevice.Viewport.Width / VirtualWidth,
                (float)m_graphics.GraphicsDevice.Viewport.Width / VirtualWidth,
                1f);
        }

        private static void ApplyChanges()
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (m_isFullScreen == false)
            {
                if ((DeviceWidth != m_graphics.PreferredBackBufferWidth) && (DeviceHeight != m_graphics.PreferredBackBufferHeight))
                {
                    m_graphics.PreferredBackBufferWidth = DeviceWidth;
                    m_graphics.PreferredBackBufferHeight = DeviceHeight;
                    m_graphics.IsFullScreen = false;
                    m_graphics.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == DeviceWidth) && (dm.Height == DeviceHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        m_graphics.PreferredBackBufferWidth = DeviceWidth;
                        m_graphics.PreferredBackBufferHeight = DeviceHeight;
                        m_graphics.IsFullScreen = true;
                        m_graphics.ApplyChanges();
                    }
                }
            }

            m_dirty = true;

            DeviceWidth = m_graphics.PreferredBackBufferWidth;
            DeviceHeight = m_graphics.PreferredBackBufferHeight;
        }

        private static void CalculateViewport()
        {
            float targetAspectRatio = (float)VirtualWidth / VirtualHeight;

            // Try letterbox
            int width = m_graphics.PreferredBackBufferWidth;
            var height = (int)((width / targetAspectRatio) + .5f);

            // If it doesn't fit then use pillarbox
            if (height > m_graphics.PreferredBackBufferHeight)
            {
                height = m_graphics.PreferredBackBufferHeight;
                width = (int)((height * targetAspectRatio) + .5f);
            }

            var viewport = new Viewport
            {
                X = (m_graphics.PreferredBackBufferWidth / 2) - (width / 2),
                Y = (m_graphics.PreferredBackBufferHeight / 2) - (height / 2),
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };

            ViewportX = viewport.X;
            ViewportY = viewport.Y;
            ViewportWidth = (width > 1) ? viewport.Width : DeviceWidth;
            ViewportHeight = (height > 1) ? viewport.Height : DeviceHeight;

            m_graphics.GraphicsDevice.Viewport = viewport;

            m_dirty = true;

#if XBOX
            //Viewport tempViewport = new Viewport(0, 0, DeviceWidth, DeviceHeight);
            //Rectangle temp = tempViewport.TitleSafeArea;
            //// nondidju ! On va pas se limiter à un title safe area calibré sans bandes noires ! Let's correct this sh*t. 
            //if (Resolution.ViewportX >= tempViewport.TitleSafeArea.Left) temp.X = 0;
            //else temp.X = tempViewport.TitleSafeArea.Left - Resolution.ViewportX;
            //if (Resolution.ViewportY >= tempViewport.TitleSafeArea.Top) temp.Y = 0;
            //else temp.Y = tempViewport.TitleSafeArea.Top - Resolution.ViewportY;

            //if (Resolution.ViewportX + Resolution.ViewportWidth <= tempViewport.TitleSafeArea.X + tempViewport.Width) temp.Width = 1024 - temp.Left;
            //else temp.Width = 1024 - (DeviceWidth - (tempViewport.TitleSafeArea.X + tempViewport.TitleSafeArea.Width)) - temp.Left;
            //if (Resolution.ViewportY + Resolution.ViewportHeight <= tempViewport.TitleSafeArea.Y + tempViewport.Height) temp.Height = 768 - temp.Top;
            //temp.Height = 768 - (DeviceHeight - (tempViewport.TitleSafeArea.Y + tempViewport.TitleSafeArea.Height)) - temp.Top;

            //TGPAContext.Instance.TitleSafeArea = temp;
            //// like that eh !
#endif
        }
    }
}