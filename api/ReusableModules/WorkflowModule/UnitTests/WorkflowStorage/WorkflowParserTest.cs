using System.Linq;
using WorkflowModule.WorkflowStorage;
using Xunit;

namespace UnitTests.WorkflowStorage
{
    public class WorkflowParserTest
    {
        [Fact]
        public void Should_Parse_Basic_WFS_Data()
        {
            var yamlContent = @"
id: SubmissionsWorkflow
name: Submission FSM
description: Imaginary submission for presenting FSM definition

version: 1


states:
  - Draft
  - ManagerReview
  - CeoReview
  - InMeeting
  - VotingAllowed
  - WaitingForVotes
  - VotingFinished
  - Closed
";
            var workflowParser = new WorkflowParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);

            Assert.Equal("SubmissionsWorkflow", workflowDescriptor.Id);
            Assert.Equal("Submission FSM", workflowDescriptor.Name);
            Assert.Equal("Imaginary submission for presenting FSM definition", workflowDescriptor.Description);
            Assert.Equal(1, workflowDescriptor.Version);

            Assert.Contains("Draft", workflowDescriptor.States);
            Assert.Contains("ManagerReview", workflowDescriptor.States);
            Assert.Contains("CeoReview", workflowDescriptor.States);
            Assert.Contains("InMeeting", workflowDescriptor.States);
            Assert.Contains("VotingAllowed", workflowDescriptor.States);
            Assert.Contains("WaitingForVotes", workflowDescriptor.States);
            Assert.Contains("VotingFinished", workflowDescriptor.States);
            Assert.Contains("Closed", workflowDescriptor.States);
        }

        [Fact]
        public void Should_Parse_EventDescriptors()
        {
            var yamlContent = @"
events:
  - name: SubmissionCreated
    inputs:
      title: string
      description?: string # not mandatory
      meetingTypeId: MeetingTypeId
      agendaItemType: AgendaItemType
    validators:
      - type: IsGreaterThan
        params: [EVENT_INPUTS.title, 5]
    reducer: 
      type: PasteInputs
";

            var workflowParser = new WorkflowParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);

            Assert.Single(workflowDescriptor.EventDescriptors);

            var firstEventDescriptor = workflowDescriptor.EventDescriptors.First();

            Assert.Equal("SubmissionCreated", firstEventDescriptor.Name);
            Assert.Equal("string", firstEventDescriptor.Inputs["title"]);
            Assert.Equal("string", firstEventDescriptor.Inputs["description?"]);
            Assert.Equal("MeetingTypeId", firstEventDescriptor.Inputs["meetingTypeId"]);
            Assert.Equal("AgendaItemType", firstEventDescriptor.Inputs["agendaItemType"]);
            Assert.Single(firstEventDescriptor.ValidatorDescriptors);

            var firstValidator = firstEventDescriptor.ValidatorDescriptors.First();

            Assert.Equal("IsGreaterThan", firstValidator.Type);
            Assert.Equal(2, firstValidator.Params.Count());
            Assert.Contains("EVENT_INPUTS.title", firstValidator.Params);
            Assert.Contains("5", firstValidator.Params);

            Assert.Equal("PasteInputs", firstEventDescriptor.ReducerDescriptor.Type);
            Assert.Empty(firstEventDescriptor.ReducerDescriptor.Params);
        }

        [Fact]
        public void Should_Parse_EventDescriptors_Reducer_With_Params()
        {
            var yamlContent = @"
events:
  - name: SubmissionCreated
    reducer: 
      type: SampleNamespace.SampleReducer
      params: [EVENT_INPUTS.test, 42, ""Test""]
";
            var workflowParser = new WorkflowParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);
            var firstEventDescriptor = workflowDescriptor.EventDescriptors.First();

            Assert.Equal("SampleNamespace.SampleReducer", firstEventDescriptor.ReducerDescriptor.Type);
            Assert.Contains("EVENT_INPUTS.test", firstEventDescriptor.ReducerDescriptor.Params);
            Assert.Contains("42", firstEventDescriptor.ReducerDescriptor.Params);
            Assert.Contains("Test", firstEventDescriptor.ReducerDescriptor.Params);
        }
    }
}
