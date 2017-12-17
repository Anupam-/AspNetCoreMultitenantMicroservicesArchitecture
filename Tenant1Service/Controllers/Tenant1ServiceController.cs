using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proxies;
using ServiceResultModels;

namespace Tenant1Service.Controllers
{
    [Route("api/[controller]")]
    public class Tenant1ServiceController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            return FakeUsers.All("tenant1");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
