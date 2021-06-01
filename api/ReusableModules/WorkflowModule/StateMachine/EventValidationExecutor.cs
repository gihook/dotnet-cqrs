using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class EventValidationExecutor : IEventValidationExecutor
    {
        private readonly IWorkflowDefinitionHelper _workflowDefinitionHelper;
        private readonly IValidatorTranslator _validatiorTranslator;
        private readonly IParameterTranslator _parameterTranslator;

        public EventValidationExecutor(IWorkflowDefinitionHelper workflowDefinitionHelper, IValidatorTranslator validatorTranslator, IParameterTranslator parameterTranslator)
        {
            _workflowDefinitionHelper = workflowDefinitionHelper;
            _validatiorTranslator = validatorTranslator;
            _parameterTranslator = parameterTranslator;
        }

        public IEnumerable<ValidationError> ValidateEvent(StateInfo currentState, EventPayload payload, string workflowId)
        {
            var eventDataWithState = new EventDataWithState
            {
                EventPayload = payload,
                StateInfo = currentState,
                WorkflowId = workflowId
            };

            var errors = new List<ValidationError>();

            if (EventIsOutOfOrder(eventDataWithState)) errors.Add(ValidationError.OrderNumberConfilict());

            var eventIsAllowed = _workflowDefinitionHelper
                                     .EventIsAllowed(eventDataWithState);

            if (!eventIsAllowed)
            {
                errors.Add(ValidationError.EventNotAllowed());
            }

            var eventDescriptor = _workflowDefinitionHelper.GetEventDescriptor(payload.EventName, workflowId);

            var requiredParameterErrors = RequiredErrors(eventDescriptor.Inputs, payload.Data);
            errors.AddRange(requiredParameterErrors);

            var typeParameterErrors = TypeParameterErrors(eventDescriptor.Inputs, payload.Data);
            errors.AddRange(typeParameterErrors);

            var validationFunctionErrors = GetValidationFunctionErrors(eventDescriptor.ValidatorDescriptors, eventDataWithState);
            errors.AddRange(validationFunctionErrors);

            return errors;
        }

        private bool EventIsOutOfOrder(EventDataWithState eventDataWithState)
        {
            var currentState = eventDataWithState.StateInfo;
            var payload = eventDataWithState.EventPayload;

            return currentState.CurrentOrderNumber + 1 != payload.OrderNumber;
        }

        private IEnumerable<ValidationError> GetValidationFunctionErrors(IEnumerable<Descriptors.InputValidatorDescriptor> inputDescriptors, EventDataWithState eventDataWithState)
        {
            return inputDescriptors.Where(inputDescriptor =>
            {
                var validator = _validatiorTranslator.GetValidator(inputDescriptor);
                var parameters = ParseValidatorParameters(inputDescriptor.Params, eventDataWithState);
                var isValid = validator.Invoke(parameters);

                return !isValid;
            }).Select(id =>
            {
                return ValidationError.ValidatiorFunctionError(id.Type);
            });
        }

        private object[] ParseValidatorParameters(IEnumerable<string> descriptiveParameters, EventDataWithState eventDataWithState)
        {
            return descriptiveParameters.Select(dp => _parameterTranslator.GetParameterValue(dp, eventDataWithState)).ToArray();
        }

        private IEnumerable<ValidationError> TypeParameterErrors(Dictionary<string, string> inputs, JObject data)
        {
            foreach (var kvp in inputs)
            {
                var paramName = kvp.Key.Replace("?", "");
                var type = kvp.Value;

                if (data[paramName] == null) continue;

                if (CanParse(type, data[paramName])) continue;

                yield return ValidationError.TypeParameterError(paramName);
            }
        }

        private bool CanParse(string type, object parameterValue)
        {
            return _validatiorTranslator.CanParse(type, parameterValue);
        }

        private IEnumerable<ValidationError> RequiredErrors(Dictionary<string, string> inputParameters, JObject inputValues)
        {
            var requiredInputs = inputParameters.Keys
                                     .Where(propertyName => !propertyName.Contains('?'));

            var errors = requiredInputs.Where(propertyName =>
            {
                return inputValues[propertyName] == null;
            }).Select(ValidationError.RequiredInputParameter);

            return errors;
        }
    }
}
