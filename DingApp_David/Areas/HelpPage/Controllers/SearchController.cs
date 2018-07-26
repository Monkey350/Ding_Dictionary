using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using DingApp_David.Models;

namespace DingApp_David.Areas.HelpPage.Controllers
{
    public class SearchController : Controller
    {

        private const string APIKey = "cab72891-f003-43ef-a983-253666d45082"; //Merriam-Webster Dictionary API key. Better scope definition needed for this variable.
        private IDingDb db;

        public SearchController()
        {
            db = new DingDb();
        }

        public SearchController(IDingDb _db)
        {
            db = _db;
        }

        /// <summary>
        /// Index action. Will show a definition by default.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() //default view page
        {

            ViewBag.Title = "Ding Search";

            var model = db.Query<WordModel>().FirstOrDefault(x => x.word.Equals("word here"));

            return View(model);

        }

        /// <summary>
        /// Looks up the specified word. First checks the DB, then calls Dictionary API. If there are still no results an error message will be returned.
        /// </summary>
        /// <param name="word">The word to look up</param>
        /// <returns></returns>
        public ActionResult Search(string word) //word passed in to search for
        {
            //try DB
            var model = db.Query<WordModel>().FirstOrDefault(x => x.word.Equals(word));

            if (model == null) //not found in DB
            {
                //call the Merriam Dictionary API for the definition
                model = CallDictionary(word);

                if (model == null) //not found in Dictionary API
                {
                    ViewBag.Title = "Word Not Found";
                    ViewBag.Message = "Merriam Webster API returned no results for \"" + word + "\". Is your input valid?";
                    return View("Error");
                }
            }

            return View("Index", model);
        }

        /// <summary>
        /// Not sure if this is required but the .NET MVC tutorial I watched recommended I include this.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Calls DictionaryAPI to search for the word
        /// </summary>
        /// <param name="word">The word to search for</param>
        /// <returns></returns>
        public WordModel CallDictionary(string word) 
        {
            string definition = "";
            string URL = "https://www.dictionaryapi.com/api/v1/references/collegiate/xml/" + word + "?key=" + APIKey;
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
                    definition += defNum + ") " + defs.Item(i).InnerText.Replace(":", " : ") + " ";
                }

                var model = db.Add(new WordModel() { word = word, definitions = definition });
                return model;
            }
            else
            {
                return null;
            }

        }

    }
}