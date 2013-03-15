using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.ViewModels
{
    public class ConfigurationModel
    {
        public ModeConfiguration[] ScoreAttack { get; set; }
        public ModeConfiguration[] TimeAttack { get; set; }
        public ModeConfiguration[] Survival { get; set; }
        public ModeConfiguration[] Versus { get; set; }

        public ModeConfiguration GetValue(ModesEnum mode, DifficultiesEnum difficulty)
        {
            if (mode == ModesEnum.ScoreAttack)
            {
                return ScoreAttack.Where(m => m.Difficulty == difficulty).FirstOrDefault();
            }
            else if (mode == ModesEnum.TimeAttack)
            {
                return TimeAttack.Where(m => m.Difficulty == difficulty).FirstOrDefault();
            }
            else if (mode == ModesEnum.Survival)
            {
                return Survival.Where(m => m.Difficulty == difficulty).FirstOrDefault();
            }
            else if (mode == ModesEnum.Versus)
            {
                return Versus.Where(m => m.Difficulty == difficulty).FirstOrDefault();
            }

            return null;
        }
    }
}