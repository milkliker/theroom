using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EternalLove2.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/oaths
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oaths/id
        public string Get(string id)
        {
            return "some txid";
        }

        // POST api/oaths
        public string Post([FromBody]string body)
        {
            return "new txid";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
