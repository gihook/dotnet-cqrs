using System;
using Moq;
using WorkflowModule.StateMachine.Workflows;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class WorkflowInstanceTest
    {
        [Fact]
        public void CurrentStepIndex_should_be_0_upon_initialization()
        {
            var userId = Guid.NewGuid();
            var firstStep = new SingleUserStep(userId);
            firstStep.Id = "first-step";
            var steps = new Step[] { firstStep };
            var instance = new WorkflowInstance();
            instance.Steps = steps;

            Assert.Equal(0, instance.CurrentStepIndex);
            Assert.Equal(firstStep.Id, instance.CurrentStep.Id);
        }

        [Fact]
        public void CurrentStepIndex_should_be_1_when_first_step_is_completed()
        {
            var userId = Guid.NewGuid();
            var firstStepMock = new Mock<Step>();
            firstStepMock.Setup(x => x.IsCompleted).Returns(true);

            var firstStep = firstStepMock.Object;
            firstStep.Id = "first-step";

            var secondStepMock = new Mock<Step>();
            secondStepMock.Setup(x => x.IsCompleted).Returns(false);

            var secondStep = secondStepMock.Object;
            firstStep.Id = "second-step";

            var steps = new Step[] { firstStep, secondStep };
            var instance = new WorkflowInstance();
            instance.Steps = steps;

            Assert.Equal(1, instance.CurrentStepIndex);
            Assert.Equal(secondStep.Id, instance.CurrentStep.Id);
        }

        [Fact]
        public void AvailableActions_should_return_SingleUserStep_actions()
        {
            var userId = Guid.NewGuid();
            var firstStep = new SingleUserStep(userId);
            firstStep.Id = "first-step";

            var instance = new WorkflowInstance();
            var steps = new Step[] { firstStep };
            instance.Steps = steps;

            Assert.Collection(instance.AvailableActions, action =>
            {
                Assert.Equal("GoToNextStep", action);
            });
        }

        [Fact]
        public void AvailableActions_should_return_ConsensusStep_actions()
        {
            var userId = Guid.NewGuid();
            var firstStep = new SingleUserStep(userId);
            firstStep.Id = "first-step";

            var secondStep = new ConsensusStep(new Guid[] { userId });
            secondStep.Id = "second-step";

            var instance = new WorkflowInstance();
            var steps = new Step[] { firstStep, secondStep };
            instance.Steps = steps;

            firstStep.GoToNextStep();

            Assert.Equal("second-step", instance.CurrentStep.Id);

            Assert.Collection(instance.AvailableActions, action =>
            {
                Assert.Equal("Vote", action);
            });
        }

        [Fact]
        public void CurrentStepIndex_should_be_1_when_GoToNextStep_is_executed()
        {
            var userId = Guid.NewGuid();
            var firstStep = new SingleUserStep(userId);
            firstStep.Id = "first-step";

            var secondStep = new ConsensusStep(new Guid[] { userId });
            secondStep.Id = "second-step";

            var instance = new WorkflowInstance();
            var steps = new Step[] { firstStep, secondStep };
            instance.Steps = steps;

            instance.ExecuteAction("GoToNextStep", new object[] { });

            Assert.Equal("second-step", instance.CurrentStep.Id);
        }

        /* [Fact] */
        /* public void Decision_should_be_taken_from_last_step() */
        /* { */
        /*     var userId = Guid.NewGuid(); */
        /*     var firstStepMock = new Mock<Step>(); */
        /*     firstStepMock.Setup(x => x.StepState).Returns(StepState.Approved); */

        /*     var firstStep = firstStepMock.Object; */
        /*     firstStep.Id = "first-step"; */

        /*     var secondStepMock = new Mock<Step>(); */
        /*     secondStepMock.Setup(x => x.StepState).Returns(StepState.Rejected); */

        /*     var secondStep = secondStepMock.Object; */
        /*     firstStep.Id = "second-step"; */

        /*     var steps = new Step[] { firstStep, secondStep }; */
        /*     var instance = new WorkflowInstance(); */
        /*     instance.Steps = steps; */

        /*     Assert.Equal(WorkflowDecision.Rejected, instance.Decision); */
        /* } */
    }
}
