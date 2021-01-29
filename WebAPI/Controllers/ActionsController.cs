using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ActionsController : ControllerBase
    {
        [HttpGet("query/{actionName}")]
        public IActionResult ExecuteQuery(string actionName)
        {
            var parameters = Request.Query;
            var dictionary = parameters.ToDictionary(x => x.Key, x => x.Value.First() as object);
            var actionDescription = new ActionDescription
            {
                ActionName = actionName,
                Parameters = dictionary
            };

            return Ok(actionDescription);
        }
    }
}

