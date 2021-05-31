using Newtonsoft.Json.Linq;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine
{
    public class ParameterTranslator : IParameterTranslator
    {
        public object GetEventInputParameterValue(string encodedParameter, JObject data)
        {
            return 42;
        }
    }
}
