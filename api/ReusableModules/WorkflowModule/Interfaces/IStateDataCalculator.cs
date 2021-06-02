using System;
using Newtonsoft.Json.Linq;

namespace WorkflowModule.Interfaces
{
    // TODO: check if obsolete
    interface IStateDataCalculator
    {
        JObject CalculateCurrentStateData(Guid aggregateId);
        string LastExecutedEvent(Guid aggregateId);
    }
}
