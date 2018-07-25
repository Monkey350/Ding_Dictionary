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
            //lookup word
            string definition = "";
            string connectionString =
                "Data Source=.\\SQLEXPRESS;Initial Catalog=Ding_Dictionary;Integrated Security=True";
            SqlConnection sql = new SqlConnection(connectionString);
            try
            {
                sql.Open();
                //log "connection successful"
            }
            catch (Exception e)
            {
                //log "connection failed"
                return "cannot connect to DB";
            }

            if (!string.IsNullOrEmpty(word)) //also input validation here
            {

                //search DB

                SqlDataReader dataReader;
                string sqlStatement = "select dbo.dictionary.definitions from dbo.dictionary where dbo.dictionary.word='" + word + "'";
                SqlCommand command = new SqlCommand(sqlStatement, sql);
                dataReader = command.ExecuteReader();

                if (dataReader.HasRows) //found in DB
                {
                    dataReader.Read();
                    definition = (string)dataReader.GetValue(0);
                    return definition;
                }
                else //not found in DB, call Dictionary API
                {
                    //call the Merriam Dictionary API for the definition
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
                            definition += "\'" + defNum + ") " +
                                         defs.Item(i).InnerText.Replace("\'", "\'\'").Substring(1) + "\'";
                            if (defNum < defs.Count) //no newline if last definition
                            {
                                definition += " + CHAR(13) + CHAR(10) + ";
                            }
                        }

                        sqlStatement = "insert into (word, definitions) values (" + word + ", " + definition + ")";
                        command = new SqlCommand(sqlStatement, sql);
                        dataReader.Close();
                        dataReader = command.ExecuteReader();

                        //for now
                        return "";
                    }
                    else //not found in Dictionary API
                    {
                        //log "word not found"
                        return "no definition found for the entered term: " + word + " .";
                    }
                }
            }
            else
            {
                //log "no valid word entered"
                return "Please enter a valid word to search";
            }
        }

    }
}
