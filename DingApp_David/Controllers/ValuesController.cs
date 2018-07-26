using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DingApp_David.Controllers
{
    public class ValuesController : ApiController
    {
        private const string APIKey = "cab72891-f003-43ef-a983-253666d45082"; //Merriam-Webster Dictionary API key. Better scope definition needed for this variable.
        
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
            return !string.IsNullOrEmpty(word) ? word : "";
        }

    }
}
