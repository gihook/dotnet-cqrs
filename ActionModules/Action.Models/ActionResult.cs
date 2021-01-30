using System.Collections.Generic;

namespace Action.Models
{
    public class ActionResult
    {
        public ActionResultStatus ResultStatus { get; set; }
        public object ResultData { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        private ActionResult() { }

        public static ActionResult BadRequest(IEnumerable<ValidationError> errors)
        {
            return new ActionResult()
            {
                ResultStatus = ActionResultStatus.BadRequest,
                ValidationErrors = errors
            };
        }

        public static ActionResult OkRequest(object data)
        {
            return new ActionResult()
            {
                ResultStatus = ActionResultStatus.Ok,
                ResultData = data
            };
        }


        public static ActionResult Unauthorized()
        {
            return new ActionResult()
            {
                ResultStatus = ActionResultStatus.Unauthorized,
            };
        }
    }
}
