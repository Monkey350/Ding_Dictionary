using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Configuration;
using DingApp_David.Models;
using DingApp_David.Services;

namespace DingApp_David.Areas.HelpPage.Controllers
{
    public class SearchController : Controller
    {

        private ILookupService lookupService;

        public SearchController()
        {
            lookupService = new LookupService();
        }

        public SearchController(ILookupService _lookupService)
        {
            lookupService = _lookupService;
        }

        /// <summary>
        /// Index action. Will show a definition by default.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() //default view page
        {

            ViewBag.Title = "Ding Search";

            var model = lookupService.WordLookup("word here");

            return View(model);

        }

        /// <summary>
        /// Looks up the specified word. First checks the DB, then calls Dictionary API. If there are still no results an error message will be returned.
        /// </summary>
        /// <param name="word">The word to look up</param>
        /// <returns></returns>
        public ActionResult Search(string word) //word passed in to search for
        {
            //LookupService tries DB then Dictionary API
            var model = lookupService.DbLookup(word);

            if (model == null)
            {
                model = lookupService.APILookup(word);
            }

            if (model == null) //not found in DB nor Dictionary API
            {
                ViewBag.Title = "Word Not Found";
                ViewBag.Message = $"Merriam Webster API returned no results for \"{word}\". Is your input valid?";
                return View("Error");
            }

            return View("Index", model);
        }

    }
}