using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using IntegrationTests.WorkflowModule.SampleAggregates;
using IntegrationTests.WorkflowModule.SampleReducers;
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
        public async Task WorkflowHandler_should_return_NULL_STATE_initially()
        {
            var workflowHandler = GetWorkflowHandler();

            var result = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("NULL_STATE", result.State);
        }

        [Fact]
        public async Task WorkflowHandler_should_move_to_Draft_after_SubmissionCreated_event()
        {
            var workflowHandler = GetWorkflowHandler();
            var title = "Test Submission 1";
            var eventPayload = new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SubmissionCreated",
                Data = JObject.FromObject(new { title }),
                OrderNumber = 1,
                EventExecutor = Executor
            };

            await workflowHandler.ExecuteEvent(eventPayload, WorkflowId);

            var newState = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("Draft", newState.State);
            Assert.Equal(title, (newState.StateData as SubmissionStateData).Title);
            Assert.Equal("", (newState.StateData as SubmissionStateData).Description);
            Assert.Equal(Executor.UserId, (newState.StateData as SubmissionStateData).CreatorUserId);
        }

        [Fact]
        public async Task WorkflowHandler_should_move_to_ManagerReview_when_summary_with_fdoa_is_saved()
        {
            var workflowHandler = GetWorkflowHandler();
            var title = "Test Submission 1";
            var firstPayload = new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SubmissionCreated",
                Data = JObject.FromObject(new { title }),
                OrderNumber = 1,
                EventExecutor = Executor
            };

            await workflowHandler.ExecuteEvent(firstPayload, WorkflowId);

            var secondPayload = new EventPayload()
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

            await workflowHandler.ExecuteEvent(secondPayload, WorkflowId);

            var stateInfo = await workflowHandler.GetCurrentStateInfo(AggregateId, WorkflowId);

            Assert.Equal("ManagerReview", stateInfo.State);
            Assert.Equal(title, (stateInfo.StateData as SubmissionStateData).Title);
            var summary = (stateInfo.StateData as SubmissionStateData).Summary;

            Assert.Equal("testFdoa", summary["General"]["Fdoa"].ToString());
        }

        private WorkflowHandler GetWorkflowHandler(IEventStore inputEventStore = null)
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation);

            var specificationsLocation = Path.Combine(assemblyFolder, "../../../WorkflowModule/WorkflowSpecifications");
            var definitionLoader = new FileWorkflowDefinitionLoader(specificationsLocation);
            var eventStore = inputEventStore ?? new InMemoryEventStore();

            var workflowHandlerFactory = new WorkflowHandlerFactory(eventStore, definitionLoader);

            workflowHandlerFactory.RegisterAllReducersFromAssembly<ExampleUsecaseTest>();

            return workflowHandlerFactory.CreateWorkflowHandler();
        }
    }
}
