using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionCore;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionParser _actionParser;
        private readonly IActionExecutor _actionExecutor;
        private readonly ActionInfoProvider _actionInfoProvider;

        public ActionsController(IActionParser actionParser, IActionExecutor actionExecutor, ActionInfoProvider actionInfoProvider)
        {
            _actionParser = actionParser;
            _actionExecutor = actionExecutor;
            _actionInfoProvider = actionInfoProvider;
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

            var executor = GetExecutor();
            var action = _actionParser.CreateAction(actionDescription);
            var result = _actionExecutor.Execute(action, executor);

            return Ok(result);
        }

        [HttpPost("command/{actionName}")]
        public async Task<IActionResult> ExecuteCommand(string actionName)
        {
            var body = await ReadBody();
            var executor = GetExecutor();
            var action = _actionParser.ParseJson(actionName, body);
            var result = _actionExecutor.Execute(action, executor);

            return Ok(result);
        }

        [HttpGet("info/{actionName}")]
        public IActionResult GetActionInfo(string actionName)
        {
            var info = _actionInfoProvider.GetActionInfo(actionName);

            return Ok(info);
        }

        private async Task<string> ReadBody()
        {
            var request = Request.Body;
            var body = await new StreamReader(request).ReadToEndAsync();

            return body;
        }

        private Executor GetExecutor()
        {
            // TODO: read from access token
            return new Executor();
        }
    }
}

