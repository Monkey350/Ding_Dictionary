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
        DingDb db = new DingDb();

        public ActionResult Index() //default view page
        {

            ViewBag.Title = "Ding Search";

            var model = db.Words.FirstOrDefault(x => x.word.Equals("kek"));

            return View(model);

        }

        public ActionResult Search(string word) //word passed in to search for
        {
            //try DB
            var model = db.Words.FirstOrDefault(x => x.word.Equals(word));

            if (model == null) //not found in DB
            {
                //call the Merriam Dictionary API for the definition
                string definition = "";
                string URL = "https://www.dictionaryapi.com/api/v1/references/collegiate/xml/" + word + "?key=" + APIKey;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(URL);
                request.Method = "GET";
                XmlDocument xmlResponse = new XmlDocument();
                using (HttpWebResponse resp = (HttpWebResponse) request.GetResponse())
                {
                    xmlResponse.Load(resp.GetResponseStream());
                }

                XmlNodeList defs = xmlResponse.GetElementsByTagName("dt");

                if (defs.Count > 0) //found in Dictionary API
                {
                    for (int i = 0; i < defs.Count; i++)
                    {
                        int defNum = i + 1; //definition number
                        definition += defNum + ") " + defs.Item(i).InnerText.Replace(":", " : ") + "\r\n";
                    }

                    model = db.Words.Add(new WordModel() { word = word, definitions = definition });
                    db.SaveChanges();
                }

                if (model == null) //not found in Dictionary API
                {
                    model = new WordModel()
                    {
                        word = "Word Not Found",
                        definitions = "Merriam Webster API returned no results for \"" + word + "\". Is your input valid?"
                    };
                }
            }

            return View("Index", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}