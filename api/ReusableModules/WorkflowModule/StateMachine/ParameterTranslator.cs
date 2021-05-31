using Newtonsoft.Json.Linq;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine
{
    public class ParameterTranslator : IParameterTranslator
    {
        public object GetEventInputParameterValue(string encodedParameter, JObject data)
        {
            var token = data.SelectToken(encodedParameter);

            return token.ToObject<object>();
        }
    }
}
