using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SuperKoikoukesse.Webservice.Core.Games
{
    public class GameInfo
    {
        public int GameId { get; set; }

        public string ImagePath { get; set; }

        public string TitlePAL { get; set; }

        /// <summary>
        /// Build object from XML element
        /// </summary>
        /// <param name="xml"></param>
        public void FromXml(XElement xml)
        {
            GameId = Convert.ToInt32(xml.Element("gameId").Value);
            ImagePath = xml.Element("imagePath").Value;
            TitlePAL = xml.Element("titlePAL").Value;
        }

        /// <summary>
        /// Return the XML corresponding to the object
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            XElement e = new XElement("game");

            e.Add(new XElement("gameId", GameId));
            e.Add(new XElement("imagePath", ImagePath));
            e.Add(new XElement("titlePAL", TitlePAL));

            return e;
        }
    }
}
