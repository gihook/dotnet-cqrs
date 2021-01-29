using System.Collections.Generic;
using System.Linq;

namespace WebAPI.ActionModels
{
    public class ActionResult
    {
        public ActionResultStatus ResultStatus { get; set; }
        public object ResultData { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        public ActionResult()
        {
            ValidationErrors = Enumerable.Empty<ValidationError>();
        }
    }
}
