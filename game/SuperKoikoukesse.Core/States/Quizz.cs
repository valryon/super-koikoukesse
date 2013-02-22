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
        // Questions data
        private List<Question> m_questions;
        private int m_currentQuestionIndex;

        public Quizz(GameContext context)
            : base(context)
        {
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
            context.Content.LoadTexture(Constants.QuizzBackgroundImageAsset);

            // Load question content
            foreach (Question q in m_questions)
            {
                q.LoadContent();

                foreach (Answer a in q.Answers)
                {
                    a.LoadContent();
                }
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
            context.SpriteBatch.Draw(Constants.QuizzBackgroundImageAsset, context.ViewportSize, Color.White);

            // Draw the joker button
            context.SpriteBatch.Draw(Constants.QuizzButtonAsset, new Rectangle((int)Constants.QuizzJokerButtonLocation.X, (int)Constants.QuizzJokerButtonLocation.Y - Constants.QuizzButtonHeight, Constants.QuizzButtonWidth, Constants.QuizzButtonHeight), Color.Pink);

            // Draw the question
            Question m_currentQuestion = m_questions[m_currentQuestionIndex];
            m_currentQuestion.Draw(Constants.QuizzImageFrame);

            // Draw the answers
            Vector2 location = Constants.QuizzJokerButtonLocation;
            foreach (Answer answer in m_currentQuestion.Answers)
            {
                answer.Draw(location);
                location.Y += Constants.QuizzButtonHeight;
            }

            context.SpriteBatch.End();
        }
    }
}
