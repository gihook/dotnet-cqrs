using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IEventReducer
    {
        object Reduce(object currentStateData, EventPayload payload);
    }
}
