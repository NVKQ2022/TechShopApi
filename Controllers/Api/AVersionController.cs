using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.IO;
using TechShopApi.Helpers;
namespace TechShopApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AVersionController : ControllerBase
    {
        VersionHelper versionHelper;

        public AVersionController(VersionHelper versionHelper)
        {
            this.versionHelper = versionHelper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Version()
        {
            var versionInfo = new
            {
                Version = versionHelper.GetBuildInfo("VERSION"),
                BuildDate = versionHelper.GetBuildInfo("BUILD_DATE"),
                CommitHash = versionHelper.GetBuildInfo("COMMIT_HASH")
            };
            return Ok(versionInfo);
        }

        
    }
}
