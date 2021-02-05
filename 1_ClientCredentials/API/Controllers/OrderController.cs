using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET
        //添加一个Scope值，表示指定范围的Scope才能访问
        [Authorize("orderScope")]
        public IActionResult Get()
        {
            var result = new[] { "order1", "order2", "order3" };
            return Ok(result);
        }
    }
}