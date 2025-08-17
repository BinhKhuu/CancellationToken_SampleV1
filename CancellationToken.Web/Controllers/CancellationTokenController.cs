using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CancellationToken.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CancellationTokenController : ControllerBase
    {
        [HttpGet("LongRunning")]
        public async Task<IActionResult> Get(System.Threading.CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Delay(5000, cancellationToken);
                return Ok();
            }
            catch (TaskCanceledException ex)
            {
                // should be called if cancellation is called
                return StatusCode(499, ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                // should not be called IF cancellation is called 
                return StatusCode(499, ex.Message);
            }
        }
        
    }
}
