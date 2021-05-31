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
            var evendData = new JObject { ["test"] = 42 };
            var result = translator.GetEventInputParameterValue(encodedParameter, evendData);

            Assert.Equal(42, result);
        }
    }
}
