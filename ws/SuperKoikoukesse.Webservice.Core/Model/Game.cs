using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    public class Game
    {
        [BsonId]
        public int GameId { get; set; }

        public string ImagePath { get; set; }

        public string TitlePAL { get; set; }

        public string TitleUS { get; set; }

        public string Platform { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public int Year { get; set; }

        public bool IsRemoved { get; set; }

        /// <summary>
        /// Build object from XML element
        /// </summary>
        /// <param name="xml"></param>
        public void FromXml(XElement xml)
        {
            GameId = Convert.ToInt32(xml.Element("GameId").Value);
            ImagePath = xml.Element("ImagePath").Value;
            TitlePAL = xml.Element("TitlePAL").Value;
            TitleUS = xml.Element("TitleUS").Value;
            Platform = xml.Element("Platform").Value;
            Genre = xml.Element("Genre").Value;
            Publisher = xml.Element("Publisher").Value;
            Year = Convert.ToInt32(xml.Element("Year").Value);
            IsRemoved = Convert.ToBoolean(xml.Element("IsRemoved").Value);
        }

        /// <summary>
        /// Return the XML corresponding to the object
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            XElement e = new XElement("game");

            e.Add(new XElement("GameId", GameId));
            e.Add(new XElement("ImagePath", ImagePath));
            e.Add(new XElement("TitlePAL", TitlePAL));
            e.Add(new XElement("TitleUS", TitleUS));
            e.Add(new XElement("Platform", Platform));
            e.Add(new XElement("Genre", Genre));
            e.Add(new XElement("Publisher", Publisher));
            e.Add(new XElement("Year", Year));
            e.Add(new XElement("IsRemoved", IsRemoved));

            return e;
        }
    }
}
