using System.Linq;
using WorkflowModule.StateMachineStorage;
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
            var workflowParser = new StateMachineParser();
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
    reducer: PasteInputs
";

            var workflowParser = new StateMachineParser();
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
            var workflowParser = new StateMachineParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);
            var firstEventDescriptor = workflowDescriptor.EventDescriptors.First();

            Assert.Empty(firstEventDescriptor.ValidatorDescriptors);
            Assert.Equal("SampleNamespace.SampleReducer", firstEventDescriptor.ReducerDescriptor.Type);
            Assert.Contains("EVENT_INPUTS.test", firstEventDescriptor.ReducerDescriptor.Params);
            Assert.Contains("42", firstEventDescriptor.ReducerDescriptor.Params);
            Assert.Contains("Test", firstEventDescriptor.ReducerDescriptor.Params);
        }

        [Fact]
        public void Should_Parse_EventTransitionDescriptors()
        {
            var yamlContent = @"
eventTransitions:
  - event: SummarySaved
    fromState: Draft
    conditionalTransition:
      - condition: IsDefined # same functions as in validators
        params: [CALCULATED_STATE_DATA.summary.general.fdoa]
        toState: ManagerReview
      # conditionalTransition will set first matched state, if none mathe this one will
      - condition: TRUE
        toState: Draft
  - event: ItemAdded
    fromState: ManagerReview
    toState: FinalState
";
            var workflowParser = new StateMachineParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);
            var eventTransitionDescriptors = workflowDescriptor.EventTransitionDescriptors;

            Assert.Collection(eventTransitionDescriptors, item =>
            {
                Assert.Equal("SummarySaved", item.Event);
                Assert.Equal("Draft", item.FromState);

                Assert.Collection(item.ConditionalTransitions, transition =>
                {
                    Assert.Equal("IsDefined", transition.Condition);
                    Assert.Contains("CALCULATED_STATE_DATA.summary.general.fdoa", transition.Params);
                    Assert.Equal("ManagerReview", transition.ToState);
                }, transition =>
                {
                    Assert.Equal("TRUE", transition.Condition);
                    Assert.Empty(transition.Params);
                    Assert.Equal("Draft", transition.ToState);
                });
            }, item =>
            {
                Assert.Equal("ItemAdded", item.Event);
                Assert.Equal("ManagerReview", item.FromState);

                Assert.Collection(item.ConditionalTransitions, transition =>
                {
                    Assert.Collection(item.ConditionalTransitions, transition =>
                    {
                        Assert.Equal("TRUE", transition.Condition);
                        Assert.Empty(transition.Params);
                        Assert.Equal("FinalState", transition.ToState);
                    });
                });
            });
        }
    }
}
