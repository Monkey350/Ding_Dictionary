using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net;
using System.Configuration;
using DingApp_David.Models;

namespace DingApp_David.Services
{
    public class LookupService
    {

        private IDingDb db;
        private readonly string APIKey = ConfigurationManager.AppSettings["Dictionary_API_Key"]; //from Web.config

        public LookupService()
        {
            db = new DingDb();
        }

        public LookupService(IDingDb _db)
        {
            db = _db;
        }

        public WordModel DbLookup(string word)
        {
            WordModel model = db.Query<WordModel>().FirstOrDefault(x => x.word.Equals(word));
            return model;
        }

        public WordModel APILookup(string word)
        {
            string definition = "";
            string URL = $"https://www.dictionaryapi.com/api/v1/references/collegiate/xml/{word}?key={APIKey}";
            XmlDocument xmlResponse = new XmlDocument();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                xmlResponse.Load(resp.GetResponseStream());
            }

            //definitions enclosed in <dt> tags in XML reponse from Dictionary API
            XmlNodeList defs = xmlResponse.GetElementsByTagName("dt");

            if (defs.Count > 0) //found in Dictionary API
            {
                for (int i = 0; i < defs.Count; i++)
                {
                    int defNum = i + 1; //definition number
                    definition += $"{defNum}) {defs.Item(i).InnerText.Replace(":", " : ")} ";
                }

                WordModel model = db.Add(new WordModel() { word = word, definitions = definition });
                return model;
            }
            else
            {
                return null;
            }
        }

        public WordModel WordLookup(string word)
        {
            WordModel model = DbLookup(word);

            if (model == null)
            {
                model = APILookup(word);
            }

            return model;
        }
    }
}