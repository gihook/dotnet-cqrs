using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IParameterTranslator
    {
        object GetParameterValue(string encodedParameter, EventDataWithState eventDataWithState);
    }
}
