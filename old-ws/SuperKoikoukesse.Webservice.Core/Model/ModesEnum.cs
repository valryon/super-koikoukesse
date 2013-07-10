using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    public enum ModesEnum : int
    {
        ScoreAttack = 0,
        TimeAttack = 1,
        Survival = 2,
        Versus = 4
    }

    public static class ModesEnumEx
    {
        public static ModesEnum FromInt(int val)
        {
            return (ModesEnum)Enum.Parse(typeof(ModesEnum), val.ToString());
        }
    }
}
