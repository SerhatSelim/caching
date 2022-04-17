using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;

namespace redisApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisCacheController : ControllerBase
    {
        private readonly IRedisDatabase redisDatabase;

        public RedisCacheController(IRedisDatabase redisDatabase)
        {
            this.redisDatabase = redisDatabase;
        }

        [HttpGet]
        [Route("get-cache")]
        public async Task<IActionResult> GetCacheAsync(string key) => 
            Ok(await redisDatabase.GetAsync<string>(key));
        

        [HttpPost]
        [Route("set-cache")]
        public async Task<IActionResult> SetCacheAsync(string key, string value) =>
             Ok(await redisDatabase.AddAsync<string>(key, value, DateTimeOffset.Now.AddMinutes(2)));
         
    }
}
