using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreCodeCamp.Controllers
{
 [ApiController] [Route("api/[controller]")]
 public class OperationsController:ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OperationsController(IConfiguration configuration )
        {
            _configuration = configuration;
        }


     [HttpOptions("ReloadConfiguration")]   public ActionResult ReloadConfiguration()
        {
            try
            {
                var root = (IConfigurationRoot)_configuration;
                root.Reload();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
