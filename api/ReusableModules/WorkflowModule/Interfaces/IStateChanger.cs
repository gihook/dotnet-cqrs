using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IStateChanger
    {
        string GetNewState(EventDataWithState eventDataWithState);
    }
}
