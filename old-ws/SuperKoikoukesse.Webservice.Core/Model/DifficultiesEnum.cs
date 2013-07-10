using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    public enum DifficultiesEnum
    {
        Easy = 0,
        Hard = 1,
        Expert = 2,
        Nolife = 3
    }

    public static class DifficultiesEnumEx
    {
        public static DifficultiesEnum FromInt(int val)
        {
            return (DifficultiesEnum)Enum.Parse(typeof(DifficultiesEnum), val.ToString());
        }
    }
}
