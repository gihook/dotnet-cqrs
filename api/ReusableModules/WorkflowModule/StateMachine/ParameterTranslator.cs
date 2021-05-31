using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class ParameterTranslator : IParameterTranslator
    {
        private const string EVENT_INPUTS = "EVENT_INPUTS";
        private const string CURENT_STATE_DATA = "CURENT_STATE_DATA";

        public object GetParameterValue(string encodedParameter, EventDataWithState eventDataWithState)
        {
            if (encodedParameter.StartsWith(EVENT_INPUTS))
            {
                var path = encodedParameter.Replace(EVENT_INPUTS, String.Empty);
                var eventData = eventDataWithState.EventPayload.Data;

                return GetReturnValue(eventData, path);
            }

            if (encodedParameter.StartsWith(CURENT_STATE_DATA))
            {
                var path = encodedParameter.Replace(CURENT_STATE_DATA, String.Empty);

                var json = JsonConvert.SerializeObject(eventDataWithState.StateInfo.StateData);
                var obj = JObject.Parse(json);

                return GetReturnValue(obj, path);
            }

            if (encodedParameter.StartsWith("EVENT_EXECUTOR"))
            {
                var path = encodedParameter.Replace("EVENT_EXECUTOR", String.Empty);

                var json = JsonConvert.SerializeObject(eventDataWithState.EventPayload.EventExecutor);
                var obj = JObject.Parse(json);

                return GetReturnValue(obj, path);
            }

            return encodedParameter;
        }

        private object GetReturnValue(JObject obj, string path)
        {
            var token = obj.SelectToken(path);

            if (token == null) return null;

            return token.ToObject<object>();
        }
    }
}
