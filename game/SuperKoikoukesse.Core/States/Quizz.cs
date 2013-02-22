using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperKoikoukesse.Core.Engine.GameStates;
using Microsoft.Xna.Framework;
using SuperKoikoukesse.Core.Data;

namespace SuperKoikoukesse.Core.Engine.States
{
    /// <summary>
    /// The quizz game state
    /// </summary>
    public class Quizz : GameState
    {
        private string backgroundImageAsset = "gfxs/backgrounds/quizz_background";

        // Image location
        private Rectangle imageDestination;

        // Questions data
        private List<Question> m_questions;
        private int m_currentQuestionIndex;

        public Quizz(GameContext context)
            : base(context)
        {
            imageDestination = new Rectangle(35, 140, 500, 500);

            m_questions = new List<Question>();
            // Hardcoded questions for now

            m_questions.Add(new ImageQuestion(context)
            {
                ImageAssetName = "game_abuse",
                ImagePath = "gfxs/sprites/games/Abuse",
                Answers = new List<Answer>()
                {
                    new Answer(context,"Pouet 1",false),
                    new Answer(context,"Pouet 2",false),
                    new Answer(context,"Abuse",true),
                    new Answer(context,"Pouet 3",false)
                }
            });

            m_questions.Add(new ImageQuestion(context)
            {
                ImageAssetName = "game_airwolf",
                ImagePath = "gfxs/sprites/games/Airwolf",
                Answers = new List<Answer>()
                {
                    new Answer(context,"Pouet 1",false),
                    new Answer(context,"Pouet 2",false),
                    new Answer(context,"Pouet 3",false),
                    new Answer(context,"Airwolf",true)
                }
            });

            m_currentQuestionIndex = -1;
        }

        public override void LoadContent()
        {
            // Load background
            context.Content.LoadTexture(backgroundImageAsset);

            // Load question content
            foreach (Question q in m_questions)
            {
                q.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // First time
            if (m_currentQuestionIndex < 0)
            {
                m_currentQuestionIndex = 0;
            }

            // Quizz is over
            if (m_currentQuestionIndex >= m_questions.Count)
            {
                m_currentQuestionIndex = -1;
                // TODO Change state
            }

            // Update thr quizz
            // -- Current question
            Question m_currentQuestion = m_questions[m_currentQuestionIndex];

            m_currentQuestion.Update(gameTime);

            // -- Answers
            foreach (Answer answer in m_currentQuestion.Answers)
            {
                answer.Update(gameTime);
            }
        }

        public override void Draw()
        {
            context.SpriteBatch.Begin(Camera);

            // Draw the background
            context.SpriteBatch.Draw(backgroundImageAsset, context.ViewportSize, Color.White);

            // Draw the question
            Question m_currentQuestion = m_questions[m_currentQuestionIndex];
            m_currentQuestion.Draw(imageDestination);

            // Draw the answers
            Vector2 location = new Vector2(630, 190);
            foreach (Answer answer in m_currentQuestion.Answers)
            {
                location.Y += 110;
                answer.Draw(location);
            }

            context.SpriteBatch.End();
        }
    }
}
