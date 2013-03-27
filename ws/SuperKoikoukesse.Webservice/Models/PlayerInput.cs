using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SuperKoikoukesse.Webservice.Models
{

    [DataContract]
    public class PlayerCreateInput
    {
        [DataMember(Name = "player", IsRequired = true)]
        public string PlayerId { get; set; }

        [DataMember(Name = "ios", IsRequired = true)]
        public string Platform { get; set; }

        [DataMember(Name = "credits", IsRequired = true)]
        public int Credits { get; set; }

        [DataMember(Name = "coins", IsRequired = true)]
        public int Coins { get; set; }

    }

    [DataContract]
    public class PlayerCoinsInput
    {
        [DataMember(Name = "player", IsRequired = true)]
        public string PlayerId { get; set; }

        [DataMember(Name = "coins", IsRequired = true)]
        public int Coins { get; set; }

    }

    [DataContract]
    public class PlayerCreditsInput
    {
        [DataMember(Name = "player", IsRequired = true)]
        public string PlayerId { get; set; }

        [DataMember(Name = "credits", IsRequired = true)]
        public int Credits { get; set; }

    }
}