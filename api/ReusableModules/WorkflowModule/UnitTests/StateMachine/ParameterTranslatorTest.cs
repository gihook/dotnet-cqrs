using Newtonsoft.Json.Linq;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class ParameterTranslatorTest
    {
        [Fact]
        public void GetParameterValue_should_tranlsate_event_input_parameter()
        {
            var encodedParameter = "test";
            var translator = new ParameterTranslator();
            var evendData = new JObject { ["test"] = "testValue" };
            var result = translator.GetEventInputParameterValue(encodedParameter, evendData);

            Assert.Equal("testValue", result);
        }

        [Fact]
        public void GetParameterValue_should_translate_nested()
        {
            var encodedParameter = "test.nested";
            var translator = new ParameterTranslator();
            var evendData = JObject.Parse(@"{
                                    ""test"": {
                                         ""nested"": ""value""
                                    }
			        }");

            System.Console.WriteLine("evendData:");
            System.Console.WriteLine(evendData);
            var result = translator.GetEventInputParameterValue(encodedParameter, evendData);

            Assert.Equal("value", result);
        }
    }
}
