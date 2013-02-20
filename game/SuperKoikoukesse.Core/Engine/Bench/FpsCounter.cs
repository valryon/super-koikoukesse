using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Engine.Bench
{
    public class FpsCounter
    {
        private int _totalFrames;
        private float _elapsedTime;
        private int _fps;

        private GameContext m_context;

        public FpsCounter(GameContext context)
        {
            m_context = context;
            Initialize();
        }

        public void Initialize()
        {
            _totalFrames = 0;
            _elapsedTime = 0f;
            _fps = 0;
        }

        /// <summary>
        /// Doit etre appelé à chaque frame pour être valide
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Seconde
            if (_elapsedTime >= 1000.0f)
            {
                _fps = _totalFrames;
                _totalFrames = 0;
                _elapsedTime = 0;
            }
        }

        /// <summary>
        /// Affichage du nombre de FPS à l'écran et calcul du nombre d'affichages
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location"></param>
        public void Draw(Vector2 location)
        {
            // On compte un affichage supplémentaire
            _totalFrames++;

            Color color = Color.Chartreuse;

            if (_fps < 25) color = Color.Red;

            m_context.Camera.DrawString(m_context.ContentLoader.Font, string.Format("{0}", _fps), location, color);
        }
    }
}
