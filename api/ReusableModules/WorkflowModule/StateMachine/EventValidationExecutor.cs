using System.Collections.Generic;
using System.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class EventValidationExecutor : IEventValidationExecutor
    {
        private readonly WorkflowDefinitionHelper _workflowDefinitionHelper;
        private readonly IValidatorTranslator _validatiorTranslator;

        public EventValidationExecutor(WorkflowDefinitionHelper workflowDefinitionHelper, IValidatorTranslator validatorTranslator)
        {
            _workflowDefinitionHelper = workflowDefinitionHelper;
            _validatiorTranslator = validatorTranslator;
        }

        public IEnumerable<ValidationError> ValidateEvent(StateInfo currentState, EventPayload payload, string workflowId)
        {
            var errors = new List<ValidationError>();

            if (EventIsOutOfOrder(currentState, payload)) errors.Add(ValidationError.OrderNumberConfilict());

            var eventIsAllowed = _workflowDefinitionHelper
                                     .EventIsAllowed(payload.EventName, currentState, workflowId);

            if (!eventIsAllowed)
            {
                errors.Add(ValidationError.EventNotAllowed());
            }

            var eventDescriptor = _workflowDefinitionHelper.GetEventDescriptor(payload.EventName, workflowId);

            var requiredParameterErrors = RequiredErrors(eventDescriptor.Inputs, payload.Data);
            errors.AddRange(requiredParameterErrors);

            var typeParameterErrors = TypeParameterErrors(eventDescriptor.Inputs, payload.Data);
            errors.AddRange(typeParameterErrors);

            var validationFunctionErrors = GetValidationFunctionErrors(eventDescriptor.ValidatorDescriptors);
            errors.AddRange(validationFunctionErrors);

            return errors;
        }

        private bool EventIsOutOfOrder(StateInfo currentState, EventPayload payload)
        {
            return currentState.CurrentOrderNumber + 1 != payload.OrderNumber;
        }

        private IEnumerable<ValidationError> GetValidationFunctionErrors(IEnumerable<Descriptors.InputValidatorDescriptor> inputDescriptors)
        {
            return inputDescriptors.Where(id =>
            {
                var validator = _validatiorTranslator.GetValidator(id);
                var isValid = validator.Invoke(id.Params);

                return !isValid;
            }).Select(id =>
            {
                return ValidationError.ValidatiorFunctionError();
            });
        }

        private IEnumerable<ValidationError> TypeParameterErrors(Dictionary<string, string> inputs, Dictionary<string, object> data)
        {
            foreach (var kvp in inputs)
            {
                var paramName = kvp.Key.Replace("?", "");
                var type = kvp.Value;

                if (!data.ContainsKey(paramName)) continue;
                if (CanParse(type, data[paramName])) continue;

                yield return ValidationError.TypeParameterError(paramName);
            }
        }

        private bool CanParse(string type, object parameterValue)
        {
            return _validatiorTranslator.CanParse(type, parameterValue);
        }

        private IEnumerable<ValidationError> RequiredErrors(Dictionary<string, string> inputParameters, Dictionary<string, object> inputValues)
        {
            var requiredInputs = inputParameters.Keys
                                     .Where(propertyName => !propertyName.Contains('?'));

            var errors = requiredInputs.Where(propertyName =>
            {
                return !inputValues.ContainsKey(propertyName);
            }).Select(ValidationError.RequiredInputParameter);

            return errors;
        }
    }
}
