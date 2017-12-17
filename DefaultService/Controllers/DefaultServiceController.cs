using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceResultModels;

namespace DefaultService.Controllers
{
    [Route("api/[controller]")]
    public class DefaultServiceController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            return new List<UserModel> {
                new UserModel{Id=1,FirstName="Default",LastName="Default", Profile="Default" }
            };
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
