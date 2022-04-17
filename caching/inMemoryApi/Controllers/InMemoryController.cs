using inMemoryApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace inMemoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InMemoryController : ControllerBase
    {
        private readonly ILogger<InMemoryController> logger;
        private readonly IDistributedCache distributedCache;

        public InMemoryController(ILogger<InMemoryController> logger, IDistributedCache distributedCache)
        {
            this.logger = logger;
            this.distributedCache = distributedCache;
        }


        [HttpGet]
        [Route("get-cache")]
        public async Task<IActionResult> GetCacheAsync(string key)
        {
            var byteValue = await distributedCache.GetAsync(key);
            logger.LogInformation("byteValue", byteValue);
            return Ok(distributedCache.GetCacheStrongly<string>(key));
        }



        [HttpPost]
        [Route("set-cache")]
        public async Task<IActionResult> SetCacheAsync(string key, string value)
        {
            var jsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            var byteValue = Encoding.UTF8.GetBytes(jsonValue);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await distributedCache.SetAsync(key, byteValue, options);

            return Ok(true);
        }
    }
}
