using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IReducerTranslator
    {
        IEventReducer GetReducer(EventPayload payload, string workflowId);
    }
}
