using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Engine.Tools
{
    /// <summary>
    /// Random numbers utilities
    /// </summary>
    public class RandomHelper
    {
        private Random m_random;

        /// <summary>
        /// Random init with seed
        /// </summary>
        public RandomHelper(int seed)
        {
            m_random = new Random(seed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fMin"></param>
        /// <param name="fmax"></param>
        /// <returns></returns>
        public float GetRandomFloat(double fMin, double fmax)
        {
            return (float)(m_random.NextDouble() * (fmax - fMin) + fMin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetRandomInt(int min, int max)
        {
            return m_random.Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        public int GetRandomInt(int max)
        {
            return m_random.Next(max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector2(GetRandomFloat(xMin, xMax),
                GetRandomFloat(yMin, yMax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Vector2 GetRandomVector2(Vector2 center, double radius)
        {
            double randomTeta = (double)GetRandomFloat(0, 360);

            double x = Math.Cos(randomTeta) * radius;
            double y = Math.Sin(randomTeta) * radius;

            return new Vector2(center.X + (float)x, center.Y + (float)y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRandomTrajectory(double radius, double anglemin = 0, double anglemax = 360)
        {
            double randomTeta = (double)GetRandomFloat(anglemin, anglemax);

            double x = Math.Cos(randomTeta) * radius;
            double y = Math.Sin(randomTeta) * radius;

            return new Vector2((float)x, (float)y);
        }

    }

}
