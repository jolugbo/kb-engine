using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jolugbokb.Data;
using jolugbokb.middleware;
using jolugbokb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace jolugbokb.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private KBDBContext kbdbContext;
        private readonly ILogger<UsersController> _logger;
        private readonly UserMiddleware _userMiddleware;
        private readonly UtillMiddleware _utillMiddleware;

        public UsersController(ILogger<UsersController> logger, UserMiddleware userMiddleware, UtillMiddleware utillMiddleware, KBDBContext _kbdbContext)
        {
            kbdbContext = _kbdbContext;
            _logger = logger;
            _userMiddleware = userMiddleware;
            _utillMiddleware = utillMiddleware;
        }

        // GET: api/values
        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return  _userMiddleware.GetAllKBUserAsync();
        }

        // GET api/values/5
        [HttpGet("{userID}")]
        public async Task<User> GetAsync(string userID)
        {
            return await _userMiddleware.GetKBUser(userID);
        }

        // POST api/values
        [HttpPost]
        public async Task PostAsync([FromBody]User user)
        {
            await _userMiddleware.CreateKBUser(user);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody]User user)
        {
            await _userMiddleware.UpdateKBUser(user);
            
        }

        // DELETE api/values/5 
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
