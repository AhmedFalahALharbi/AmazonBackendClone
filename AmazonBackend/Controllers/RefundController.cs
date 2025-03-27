using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonBackend.Controllers
{
    [Route("api/refunds")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "RefundPolicy")]  // Enforce the Policy-Based Authorization
        public IActionResult ProcessRefund()
        {
            return Ok("Refund processed successfully.");
        }
    }
}
