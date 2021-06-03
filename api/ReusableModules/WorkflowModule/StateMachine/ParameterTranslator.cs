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
        private const string EVENT_EXECUTOR = "EVENT_EXECUTOR";

        public object GetParameterValue(string encodedParameter, EventDataWithState eventDataWithState)
        {
            if (encodedParameter.StartsWith(EVENT_INPUTS))
            {
                var path = encodedParameter.Replace(EVENT_INPUTS, String.Empty);
                var eventData = eventDataWithState.EventPayload.Data ?? new JObject();

                return GetReturnValue(eventData, path);
            }

            if (encodedParameter.StartsWith(CURENT_STATE_DATA))
            {
                var path = encodedParameter.Replace(CURENT_STATE_DATA, String.Empty);
                var obj = ConvertToJObject(eventDataWithState.StateInfo.StateData);

                return GetReturnValue(obj, path);
            }

            if (encodedParameter.StartsWith(EVENT_EXECUTOR))
            {
                var path = encodedParameter.Replace(EVENT_EXECUTOR, String.Empty);
                var obj = ConvertToJObject(eventDataWithState.EventPayload.EventExecutor);

                return GetReturnValue(obj, path);
            }

            return encodedParameter;
        }

        private JObject ConvertToJObject(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JObject.Parse(json);
        }

        private object GetReturnValue(JObject obj, string path)
        {
            var token = obj.SelectToken(path);

            if (token == null) return null;

            return token.ToObject<object>();
        }
    }
}
