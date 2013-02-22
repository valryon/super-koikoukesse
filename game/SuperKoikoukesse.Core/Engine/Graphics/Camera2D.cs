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
    /// A 2D camera that can move, zoom, fade in and out, shake...
    /// </summary>
    public class Camera2D
    {
        #region Constants

        /// <summary>
        /// Zooming max factor value (1 = base)
        /// </summary>
        public const float ZoomInMaxValue = 5f;
        /// <summary>
        /// Dezooming mac factor value (1 = base)
        /// </summary>
        public const float ZoomOutMaxValue = 0.3f;

        #endregion

        private Vector2 m_position; // Camera top-left corner
        private float m_rotation; // Camera Rotation
        private Matrix m_transform; // Matrix Transform
        private Rectangle m_visibilityRectangle, m_awarenessRectangle;
        private float m_zoom; // Camera Zoom

        private bool m_useBounds;
        private float m_lastZoomModifier;
        private Rectangle m_bounds;
        private Color m_fadingColor;

        // Shake
        private Vector2 m_currentShakeCooldown;

        // Fade
        private float m_currentFadeInOut, m_currentFadeDelta;
        private Action m_onFadeComplete;

        public Camera2D()
        {
            Reset();
        }

        /// <summary>
        /// Set default values for the camera
        /// </summary>
        public void Reset()
        {
            m_position = new Vector2(ResolutionHelper.DeviceWidth / 2, ResolutionHelper.DeviceHeight / 2);
            m_zoom = 1.0f;
            m_rotation = 0.0f;
            m_useBounds = false;

            ShakeSpeed = new Vector2(50f, 100f);
            m_currentShakeCooldown = ShakeSpeed;
        }

        #region Update

        /// <summary>
        /// Update camera matrix and visibility rectangle
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Bounds
            controlBounds();

            // Shake shake shake
            shakeCamera(gameTime);

            // Fade in out
            updateFadeInOut();

            // Create the camera
            m_transform = Matrix.CreateTranslation(new Vector3(-m_position.X, -m_position.Y, 0)) *
                         Matrix.Identity * /* For reliable results */
                         Matrix.CreateRotationZ(m_rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(ResolutionHelper.DeviceWidth * 0.5f, ResolutionHelper.DeviceHeight * 0.5f, 0));

            // Apply the resolution trick
            m_transform *= ResolutionHelper.ResolutionMatrix;

            // Visibility rectangle
            m_visibilityRectangle = new Rectangle
            {
                X = (int)(m_position.X - ((ResolutionHelper.DeviceWidth / 2) / Zoom)),
                Y = (int)(m_position.Y - ((ResolutionHelper.DeviceHeight / 2) / Zoom)),
                Width = (int)(ResolutionHelper.DeviceWidth / Zoom),
                Height = (int)(ResolutionHelper.DeviceHeight / Zoom)
            };

            m_awarenessRectangle = m_visibilityRectangle;
            m_awarenessRectangle.Inflate(ResolutionHelper.VirtualWidth, ResolutionHelper.VirtualHeight);
        }

        private void updateFadeInOut()
        {
            m_currentFadeInOut += m_currentFadeDelta;
            if (m_currentFadeInOut > 1.0f)
            {
                m_currentFadeInOut = 1.0f;
                m_currentFadeDelta = 0f;
                if (m_onFadeComplete != null) m_onFadeComplete();
            }
            else if (m_currentFadeInOut < 0.0f)
            {
                m_currentFadeInOut = 0.0f;
                m_currentFadeDelta = 0f;
                if (m_onFadeComplete != null) m_onFadeComplete();
            }
        }

        private void shakeCamera(GameTime gameTime)
        {
            if (ShakeFactor != Vector2.Zero)
            {
                m_currentShakeCooldown.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                m_currentShakeCooldown.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (m_currentShakeCooldown.X <= 0)
                {
                    Move(new Vector2(ShakeFactor.X, 0));
                    ShakeFactor = new Vector2(-ShakeFactor.X, ShakeFactor.Y);

                    m_currentShakeCooldown.X = ShakeSpeed.X;
                }

                if (m_currentShakeCooldown.Y <= 0)
                {
                    Move(new Vector2(0, ShakeFactor.Y));
                    ShakeFactor = new Vector2(ShakeFactor.X, -ShakeFactor.Y);

                    m_currentShakeCooldown.Y = ShakeSpeed.Y;
                }
            }
        }

        private void controlBounds()
        {
            if (m_useBounds)
            {
                // Zooming getting outside of the screen
                if (m_bounds.Contains(m_visibilityRectangle) == false)
                {
                    // Remove new zoom modifier
                    m_zoom -= m_lastZoomModifier;
                }

                // Borders
                // -- Camera get stuck if it try to go further than the border
                int x = (int)(m_position.X - ((ResolutionHelper.DeviceWidth / 2) / Zoom));
                int y = (int)(m_position.Y - ((ResolutionHelper.DeviceHeight / 2) / Zoom));

                if (x < m_bounds.Left)
                {
                    m_position.X = m_bounds.Left + (ResolutionHelper.DeviceWidth / 2 / Zoom);
                }
                else if (x + (int)(ResolutionHelper.DeviceWidth / Zoom) > m_bounds.Right)
                {
                    m_position.X = m_bounds.Right - (ResolutionHelper.DeviceWidth / 2 / Zoom);
                }

                if (y < m_bounds.Top)
                {
                    m_position.Y = m_bounds.Top + (ResolutionHelper.DeviceHeight / 2 / Zoom);
                }
                if (y + (int)(ResolutionHelper.DeviceHeight / Zoom) > m_bounds.Bottom)
                {
                    m_position.Y = m_bounds.Bottom - (ResolutionHelper.DeviceHeight / 2 / Zoom);
                }

            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Define camera bounds
        /// </summary>
        /// <param name="bounds"></param>
        public void SetBounds(Rectangle bounds)
        {
            m_bounds = bounds;
            m_useBounds = true;
        }

        /// <summary>
        /// Translate the camera
        /// </summary>
        /// <param name="amount"></param>
        public void Move(Vector2 amount)
        {
            m_position += amount;
        }

        /// <summary>
        /// Fade screen to black
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        public void FadeIn(float frameCount, Action onFadeComplete)
        {
            FadeIn(frameCount, onFadeComplete, Color.Black);
        }

        /// <summary>
        /// Fade screen to the given color
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        /// <param name="col">Color</param>
        public void FadeIn(float frameCount, Action onFadeComplete, Color col)
        {
            m_currentFadeInOut = 0.0f;
            m_currentFadeDelta = (1 / frameCount);
            m_fadingColor = col;
            m_onFadeComplete = onFadeComplete;
        }

        /// <summary>
        /// Fade screen from black to normal
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        public void FadeOut(float frameCount, Action onFadeComplete)
        {
            FadeOut(frameCount, onFadeComplete, Color.Black);
        }

        /// <summary>
        /// Fade screen from color to normal
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        /// <param name="col">Color</param>
        public void FadeOut(float frameCount, Action onFadeComplete, Color col)
        {
            m_currentFadeInOut = 1.0f;
            m_currentFadeDelta = -(1 / frameCount);
            m_fadingColor = col;
            m_onFadeComplete = onFadeComplete;
        }

        #endregion

        #region Draw Fade

        /// <summary>
        /// Display the fading effect 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawFade(GameSpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(new Rectangle(0, 0, ResolutionHelper.VirtualWidth, ResolutionHelper.VirtualHeight), m_fadingColor * m_currentFadeInOut);
        }

        #endregion

        #region Coordinates utilities

        /// <summary>
        /// Transform a Camera position into a world position
        /// </summary>  
        /// <remarks>Example: Mouse pointer on the screen to world coordinates</remarks>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ToWorldLocation(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(m_transform));
        }

        /// <summary>
        /// Transform a world position into a camera one
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ToLocalLocation(Vector2 position)
        {
            return Vector2.Transform(position, m_transform);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The computed matrix of the camera, that should be passed to a SpriteBatch.Begin call
        /// </summary>
        public Matrix CameraMatrix
        {
            get
            {
                return m_transform;
            }
        }

        /// <summary>
        /// Rectangle with accurate coordinates and size of what's visible on the screen now
        /// </summary>
        public Rectangle VisibilityRectangle
        {
            get
            {
                return m_visibilityRectangle;
            }
        }

        /// <summary>
        /// It's a bit larger than VisibilityRectangle
        /// </summary>
        public Rectangle AwarenessRectangle
        {
            get
            {
                return m_awarenessRectangle;
            }
        }

        /// <summary>
        /// Zoom in or out
        /// </summary>
        public float Zoom
        {
            get { return m_zoom; }
            set
            {
                float clampedValue = MathHelper.Clamp(value, ZoomOutMaxValue, ZoomInMaxValue);
                m_lastZoomModifier = clampedValue - m_zoom;
                m_zoom = clampedValue;
            }
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        public float Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        /// <summary>
        /// This is the center of the camera
        /// </summary>
        public Vector2 Location
        {
            get { return m_position; }
            set { m_position = value; }
        }

        /// <summary>
        /// Make the camera shake
        /// </summary>
        public Vector2 ShakeFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Shaking speed (in millisceonds)
        /// </summary>
        public Vector2 ShakeSpeed
        {
            get;
            set;
        }

        #endregion


    }
}
