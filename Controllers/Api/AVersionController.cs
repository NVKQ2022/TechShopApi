using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShopApi.Helpers;

namespace TechShopApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AVersionController : ControllerBase
    {
        private readonly VersionHelper _versionHelper;

        public AVersionController(VersionHelper versionHelper)
        {
            _versionHelper = versionHelper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Version()
        {
            var versionInfo = new
            {
                Version = _versionHelper.GetBuildInfo("VERSION"),
                BuildDate = _versionHelper.GetBuildInfo("BUILD_DATE"),
                CommitHash = _versionHelper.GetBuildInfo("COMMIT_HASH"),
                test = 2
            };
            return Ok(versionInfo);
        }
    }
}
