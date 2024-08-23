using HealthCheck.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace HealthCheck.Controllers
{
    [ApiController]
    public class HealthCheckEndpointController : ControllerBase
    {
        private static string _failInstanceName = null;  // Store in memory

        private readonly IConfiguration _configuration;
        private readonly string MachineName = Environment.MachineName;

        public HealthCheckEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("/setFailInstance")]
        public IActionResult SetFailInstanceName([FromBody] InstanceNameRequest instance)
        {
            _failInstanceName = instance.InstanceName;
            return Ok(new { Message = "FailInstanceName set successfully", _failInstanceName });
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/ping")]
        public IActionResult GetStandardHealthCheck()
        {
            // Simulate fault on the in-memory stored instance name
            if (MachineName == _failInstanceName)
            {
                return StatusCode(500);
            }

            var result = new { _failInstanceName, MachineName, HttpStatusCode = HttpStatusCode.OK };
            return Ok(result);
        }
    }
}
