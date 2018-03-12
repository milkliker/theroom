using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using EternalLove2.Models;
using EternalLove2.Services;

namespace EternalLove2.Controllers
{
    public class PostRequest
    {
        public string OathContent;
    }
    
    public class OathController : ApiController
    {
        private T Sync<T>(Task<T> call)
        {
            return Task.Run(() => call).Result;
        }

        // GET: api/Oath/5
        public async Task<Oath> Get(string id)
        {
            var service = new OathService();
            var oath = await service.GetOath(id);

            return oath;
        }

        // POST: api/Oath
        public async Task<Oath> Post([FromBody]PostRequest request)
        {
            var service = new OathService();
            var txhash = await service.Write(request.OathContent);

            Oath oath = new Oath();
            oath.OathContent = request.OathContent;
            oath.TxHash = txhash;

            return oath;
        }
        
    }
}
