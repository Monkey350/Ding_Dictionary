using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DingApp_David.Models;
using DingApp_David.Services;

namespace DingApp_David.Controllers
{
    public class ValuesController : ApiController
    {

        private ILookupService lookupService;

        public ValuesController()
        {
            lookupService = new LookupService();
        }

        public ValuesController(ILookupService _lookupService)
        {
            lookupService = _lookupService;
        }

        // GET api/values?word=word
        public JObject Get(string word)
        {
            // service
            string json = "";
            WordModel result = lookupService.DbLookup(word);

            if (result == null)
            {
                result = lookupService.APILookup(word);
            }

            if (result != null)
            {
                json = JsonConvert.SerializeObject(result);
            }
            else
            {
                string error = $"Merriam Webster API returned no results for \"{word}\". Is your input valid?";
                var errObj = new { Error = error };
                json = JsonConvert.SerializeObject(errObj);
            }

            return JObject.Parse(json);
            //return json;
        }

    }
}
