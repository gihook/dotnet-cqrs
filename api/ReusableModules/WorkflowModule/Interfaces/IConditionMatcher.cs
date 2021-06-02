using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IConditionMatcher
    {
        bool IsMatched(StateInfo stateInfo, object[] parameters);
    }
}
