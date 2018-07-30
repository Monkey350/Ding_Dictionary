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
using DingApp_David.Models;
using DingApp_David.Services;

namespace DingApp_David.Controllers
{
    public class ValuesController : ApiController
    {

        private IDingDb db;
        private LookupService lookupService;

        public ValuesController()
        {
            db = new DingDb();
            lookupService = new LookupService(db);
        }

        public ValuesController(IDingDb _db)
        {
            db = _db;
            lookupService = new LookupService(db);
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        // GET api/values?word=word
        public string Get(string word)
        {
            // service
            string json = "";
            WordModel result = lookupService.WordLookup(word);

            if (result != null)
            {
                json = JsonConvert.SerializeObject(result);
            }
            else
            {
                string error = $"Merriam Webster API returned no results for \"{word}\". Is your input valid?";
                json = JsonConvert.SerializeObject(error);
            }

            return json;
        }

    }
}
