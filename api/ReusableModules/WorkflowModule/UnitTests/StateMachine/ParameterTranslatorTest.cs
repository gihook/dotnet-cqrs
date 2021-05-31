using System;
using Newtonsoft.Json.Linq;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class ParameterTranslatorTest
    {
        [Fact]
        public void GetParameterValue_should_tranlsate_event_input_parameter()
        {
            var encodedParameter = "EVENT_INPUTS.test";
            var evendData = new JObject { ["test"] = "testValue" };
            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload { Data = evendData }
            };

            var translator = new ParameterTranslator();
            var result = translator.GetParameterValue(encodedParameter, eventDataWithState);

            Assert.Equal("testValue", result);
        }

        [Fact]
        public void GetParameterValue_should_translate_nested()
        {
            var encodedParameter = "EVENT_INPUTS.test.nested";
            var evendData = JObject.Parse(@"{
                                    ""test"": {
                                         ""nested"": ""value""
                                    }
			        }");

            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload { Data = evendData }
            };

            var translator = new ParameterTranslator();
            var result = translator.GetParameterValue(encodedParameter, eventDataWithState);

            Assert.Equal("value", result);
        }

        [Fact]
        public void GetParameterValue_should_return_null_for_non_existing_token()
        {
            var encodedParameter = "EVENT_INPUTS.test.nonExisting";
            var evendData = new JObject();

            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload { Data = evendData }
            };

            var translator = new ParameterTranslator();
            var result = translator.GetParameterValue(encodedParameter, eventDataWithState);

            Assert.Null(result);
        }

        [Fact]
        public void GetParameterValue_should_parse_current_state_parameter()
        {
            var encodedParameter = "CURENT_STATE_DATA.Sample.Nested[0]";
            var currentStateData = new { Sample = new { Nested = new string[] { "test" } } };
            var eventDataWithState = new EventDataWithState
            {
                StateInfo = new StateInfo { StateData = currentStateData }
            };

            var translator = new ParameterTranslator();
            var result = translator.GetParameterValue(encodedParameter, eventDataWithState);

            Assert.Equal("test", result);
        }

        [Fact]
        public void GetParameterValue_should_parse_event_executor_parameter()
        {
            var encodedParameter = "EVENT_EXECUTOR.UserId";
            var eventExecutor = new EventExecutor
            {
                UserId = new Guid()
            };
            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload { EventExecutor = eventExecutor }
            };

            var translator = new ParameterTranslator();
            var result = translator.GetParameterValue(encodedParameter, eventDataWithState);

            Assert.Equal("00000000-0000-0000-0000-000000000000", result);
        }
    }
}
