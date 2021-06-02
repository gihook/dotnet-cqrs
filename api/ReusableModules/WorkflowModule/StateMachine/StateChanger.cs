using System.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class StateChanger : IStateChanger
    {
        private const string TRUE_CONDITION = "TRUE";

        private readonly IWorkflowDefinitionHelper _definitionHelper;
        private readonly IValidatorTranslator _validatiorTranslator;
        private readonly IParameterTranslator _parameterTranslator;

        public StateChanger(IWorkflowDefinitionHelper definitionHelper, IValidatorTranslator validatorTranslator, IParameterTranslator parameterTranslator)
        {
            _definitionHelper = definitionHelper;
            _validatiorTranslator = validatorTranslator;
            _parameterTranslator = parameterTranslator;
        }

        public string GetNewState(EventDataWithState eventDataWithState)
        {
            var transitionDescriptor = _definitionHelper.GetMatchingEventTransitionDescriptor(eventDataWithState);

            foreach (var conditionalTransition in transitionDescriptor.ConditionalTransitions)
            {
                if (conditionalTransition.Condition == TRUE_CONDITION)
                {
                    return conditionalTransition.ToState;
                }

                var function = _validatiorTranslator.GetValidationFunction(conditionalTransition.Condition);
                var parameters = conditionalTransition.Params.Select(p => _parameterTranslator.GetParameterValue(p, eventDataWithState));

                var transitionalConditionIsMet = function(parameters.ToArray());
                if (!transitionalConditionIsMet) continue;

                return conditionalTransition.ToState;
            }

            return eventDataWithState.StateInfo.State;
        }
    }
}
