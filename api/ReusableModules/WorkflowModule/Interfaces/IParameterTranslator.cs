using Newtonsoft.Json.Linq;

namespace WorkflowModule.Interfaces
{
    public interface IParameterTranslator
    {
        object GetEventInputParameterValue(string encodedParameter, JObject data);
    }
}
