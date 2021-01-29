using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionParser _actionParser;
        private readonly IActionExecutor _actionExecutor;

        public ActionsController(IActionParser actionParser, IActionExecutor actionExecutor)
        {
            _actionParser = actionParser;
            _actionExecutor = actionExecutor;
        }

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

            var executor = new Executor();
            var action = _actionParser.CreateAction<object>(actionDescription);
            var result = _actionExecutor.Execute(action, executor);

            return Ok(result);
        }

        [HttpPost("command/{actionName}")]
        public IActionResult ExecuteCommand(string actionName, [FromBody] Dictionary<string, object> parameters)
        {
            var actionDescription = new ActionDescription
            {
                ActionName = actionName,
                Parameters = parameters
            };

            return Ok(parameters);
        }
    }
}

