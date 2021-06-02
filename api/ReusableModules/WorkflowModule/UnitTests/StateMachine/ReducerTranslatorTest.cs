using Moq;
using WorkflowModule.Descriptors;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class ReducerTranslatorTest
    {
        [Fact]
        public void GetReducer_should_return_registered_reducer_for_event()
        {
            var payload = new EventPayload() { EventName = "SentToManager" };

            var reducerMock = new Mock<IEventReducer>();
            var reducerName = "SaveChanges";

            var definitionHelper = GetWorkflowDefinitionHelper(reducerName);

            var translator = new ReducerTranslator(definitionHelper);
            translator.RegisterReducer(reducerName, reducerMock.Object);

            var reducer = translator.GetReducer(payload, "wf1");

            Assert.Equal(reducerMock.Object, reducer);
        }

        [Fact]
        public void GetReducer_should_throw_UnknownReducerException_when_reducer_is_not_registered()
        {
            var definitionHelper = GetWorkflowDefinitionHelper("UnknownReducer");
            var translator = new ReducerTranslator(definitionHelper);

            Assert.Throws<UnknownReducerException>(() =>
            {
                var reducer = translator.GetReducer(null, "wf1");
            });
        }

        private IWorkflowDefinitionHelper GetWorkflowDefinitionHelper(string reducerType)
        {
            var definitionHelperMock = new Mock<IWorkflowDefinitionHelper>();
            definitionHelperMock
                .Setup(d => d.GetEventDescriptor(It.IsAny<EventDataWithState>()))
                .Returns(new EventDescriptor()
                {
                    ReducerDescriptor = new ReducerDescriptor
                    {
                        Type = reducerType
                    }
                });

            return definitionHelperMock.Object;
        }
    }
}
