using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;
using Microsoft.AspNetCore.Mvc;
using ActionResult = Action.Models.ActionResult;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionParser _actionParser;
        private readonly IActionExecutor _actionExecutor;
        private readonly IActionInfoProvider _actionInfoProvider;

        public ActionsController(IActionParser actionParser, IActionExecutor actionExecutor, IActionInfoProvider actionInfoProvider)
        {
            _actionParser = actionParser;
            _actionExecutor = actionExecutor;
            _actionInfoProvider = actionInfoProvider;
        }

        [HttpGet("query/{actionName}")]
        public async Task<IActionResult> ExecuteQuery(string actionName)
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
            var result = await _actionExecutor.Execute(action, executor);

            return HandleResult(result);
        }

        [HttpPost("command/{actionName}")]
        public async Task<IActionResult> ExecuteCommand(string actionName)
        {
            var body = await ReadBody();
            var executor = GetExecutor();
            var action = _actionParser.ParseJson(actionName, body);
            var result = await _actionExecutor.Execute(action, executor);

            return HandleResult(result);
        }

        [HttpGet("info/{actionName}")]
        public IActionResult GetActionInfo(string actionName)
        {
            var info = _actionInfoProvider.GetActionInfo(actionName);

            return Ok(info);
        }

        [HttpGet("actions")]
        public IActionResult GetAllActions()
        {
            var infos = _actionInfoProvider.AllActionInfos();

            return Ok(infos);
        }

        private IActionResult HandleResult(ActionResult result)
        {
            if (result.ResultStatus == ActionResultStatus.Unauthorized) return Unauthorized(result);
            if (result.ResultStatus == ActionResultStatus.BadRequest) return BadRequest(result);
            if (result.ResultStatus == ActionResultStatus.Ok) return Ok(result);

            return BadRequest();
        }

        private async Task<string> ReadBody()
        {
            using (var streamReader = new StreamReader(Request.Body))
            {
                var body = await streamReader.ReadToEndAsync();

                return body;
            }

        }

        private Executor GetExecutor()
        {
            // TODO: read from access token
            var executor = new Executor();
            executor.Scopes = new string[] { "Admin" };

            return executor;
        }
    }
}

