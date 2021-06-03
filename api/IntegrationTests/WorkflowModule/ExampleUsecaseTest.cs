using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WorkflowModule.EventStore;
using WorkflowModule.Exceptions;
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
            var eventPayload = new EventPayload()
            {
                AggregateId = AggregateId,
                Timestamp = DateTime.Now,
                EventName = "SubmissionCreated",
                Data = JObject.FromObject(new { title = "Test Submission 1" }),
                OrderNumber = 1
            };

            try
            {
                await workflowHandler.ExecuteEvent(eventPayload, WorkflowId);
            }
            catch (EventValidationException e)
            {
                foreach (var error in e.ValidationErrors)
                {
                    Console.WriteLine("error: " + error.Id);
                    Console.WriteLine("name: " + error.ParameterName);

                    foreach (var param in error.Parameters)
                    {
                        Console.WriteLine(param);
                    }
                }

                throw e;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);

                throw e;
            }
        }

        private WorkflowHandler GetWorkflowHandler()
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation);

            var specificationsLocation = Path.Combine(assemblyFolder, "../../../WorkflowModule/WorkflowSpecifications");
            var definitionLoader = new FileWorkflowDefinitionLoader(specificationsLocation);
            var eventStore = new InMemoryEventStore();

            var workflowHandlerFactory = new WorkflowHandlerFactory(eventStore, definitionLoader);

            return workflowHandlerFactory.CreateWorkflowHandler();
        }
    }
}
