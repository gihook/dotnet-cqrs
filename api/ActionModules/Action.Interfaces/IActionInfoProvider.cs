using System.Collections.Generic;
using Action.Models;

namespace Action.Interfaces
{
    public interface IActionInfoProvider
    {
        IEnumerable<ActionInfo> AllActionInfos();
        ActionInfo GetActionInfo(string actionName);
    }
}
