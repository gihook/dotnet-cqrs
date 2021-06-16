using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using IntegrationTests.WorkflowModule.SampleAggregates;
using Newtonsoft.Json.Linq;
using WorkflowModule.EventStore;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using WorkflowModule.WorkflowStorage;
using Xunit;

namespace IntegrationTests
{
    public class ExampleUsecaseTest
    {
        public string WorkflowId { get; set; } = "ExampleSubmissionsWorkflow";
        public Guid AggregateId
        {
            get
            {
                return new Guid();
            }
        }

        public EventExecutor Executor
        {
            get
            {
                return new EventExecutor
                {
                    UserId = Guid.Parse("22223333-2222-2222-2222-111111111111"),
                    Hierarchy = "/"
                };
            }
        }

        [Fact]
        public async Task StateMachineHandler_should_return_NULL_STATE_initially()
        {
            var workflowHandler = GetStateMachineHandler();

            var result = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("NULL_STATE", result.State);
        }

        [Fact]
        public async Task StateMachineHandler_should_move_to_Draft_after_SubmissionCreated_event()
        {
            var workflowHandler = GetStateMachineHandler();
            var eventPayload = SubmissionCreatedPayload();

            await workflowHandler.ExecuteEvent(eventPayload, WorkflowId);

            var newState = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("Draft", newState.State);
            Assert.Equal("Test Submission 1", (newState.StateData as SubmissionStateData).Title);
            Assert.Equal("", (newState.StateData as SubmissionStateData).Description);
            Assert.Equal(Executor.UserId, (newState.StateData as SubmissionStateData).CreatorUserId);
        }

        [Fact]
        public async Task StateMachineHandler_should_move_to_ManagerReview_when_summary_with_fdoa_is_saved()
        {
            var workflowHandler = GetStateMachineHandler();
            var title = "Test Submission 1";
            var firstPayload = SubmissionCreatedPayload();

            await workflowHandler.ExecuteEvent(firstPayload, WorkflowId);

            var secondPayload = SummarySavedPayload();

            await workflowHandler.ExecuteEvent(secondPayload, WorkflowId);

            var stateInfo = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("ManagerReview", stateInfo.State);
            Assert.Equal(title, (stateInfo.StateData as SubmissionStateData).Title);
            var summary = (stateInfo.StateData as SubmissionStateData).Summary;

            Assert.Equal("testFdoa", summary["General"]["Fdoa"].ToString());
        }

        [Fact]
        public async Task StateMachineHandler_should_move_to_CeoReview_after_MoveToCeo_event()
        {
            var workflowHandler = GetStateMachineHandler();
            var firstPayload = SubmissionCreatedPayload();

            await workflowHandler.ExecuteEvent(firstPayload, WorkflowId);

            var secondPayload = SummarySavedPayload();

            await workflowHandler.ExecuteEvent(secondPayload, WorkflowId);

            var ceoUserIdAsString = "33333333-2222-2222-2222-999999999999";
            var thirdPayload = SentToCeoPayload();

            await workflowHandler.ExecuteEvent(thirdPayload, WorkflowId);

            var state = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("CeoReview", state.State);
            var ceoUserId = Guid.Parse(ceoUserIdAsString);
            Assert.Equal(ceoUserId, (state.StateData as SubmissionStateData).CeoReviewer);
        }

        private EventPayload SubmissionCreatedPayload()
        {
            var title = "Test Submission 1";
            return new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SubmissionCreated",
                Data = JObject.FromObject(new { title }),
                OrderNumber = 1,
                EventExecutor = Executor
            };
        }

        private EventPayload SummarySavedPayload()
        {
            return new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SummarySaved",
                Data = JObject.FromObject(new
                {
                    summary = new
                    {
                        General = new
                        {
                            Fdoa = "testFdoa"
                        }
                    }
                }),
                OrderNumber = 2,
                EventExecutor = Executor
            };
        }

        private EventPayload SentToCeoPayload()
        {
            var ceoUserIdAsString = "33333333-2222-2222-2222-999999999999";
            return new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SentToCeo",
                Data = JObject.FromObject(new
                {
                    ceoUserId = ceoUserIdAsString
                }),
                OrderNumber = 3,
                EventExecutor = Executor
            };
        }

        private StateMachineHandler GetStateMachineHandler(IEventStore inputEventStore = null)
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation);

            var specificationsLocation = Path.Combine(assemblyFolder, "../../../WorkflowModule/WorkflowSpecifications");
            var definitionLoader = new FileWorkflowDefinitionLoader(specificationsLocation);
            var eventStore = inputEventStore ?? new InMemoryEventStore();

            var workflowHandlerFactory = new StateMachineHandlerFactory(eventStore, definitionLoader);

            workflowHandlerFactory.RegisterAllReducersFromAssembly<ExampleUsecaseTest>();

            return workflowHandlerFactory.CreateStateMachineHandler();
        }
    }
}
