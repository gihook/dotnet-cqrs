using System;
using System.Collections.Generic;
using WorkflowModule.Descriptors;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine
{
    public class ValidatorTranslator : IValidatorTranslator
    {
        private readonly Dictionary<string, ITypeConverter> _converters;
        private readonly Dictionary<string, IInputValidator> _validators;

        public ValidatorTranslator()
        {
            _converters = new Dictionary<string, ITypeConverter>();
            _validators = new Dictionary<string, IInputValidator>();
        }

        public void RegisterConverter(string type, ITypeConverter converter)
        {
            _converters[type] = converter;
        }

        internal void RegisterValidator(string validatorName, IInputValidator validator)
        {
            _validators[validatorName] = validator;
        }

        public bool CanParse(string type, object parameterValue)
        {
            var hasConverter = _converters.ContainsKey(type);

            if (!hasConverter) return false;

            return _converters[type].CanParse(parameterValue);
        }

        public Func<object[], bool> GetValidator(InputValidatorDescriptor descriptor)
        {
            return GetFunction(descriptor.Type);
        }

        public Func<object[], bool> GetValidationFunction(string functionName)
        {
            return GetFunction(functionName);
        }

        private Func<object[], bool> GetFunction(string functionName)
        {
            var parsedName = functionName.Replace("NOT_", "");

            if (!_validators.ContainsKey(parsedName)) return x => false;

            var isNegated = functionName != parsedName;
            if (isNegated)
            {
                return x => !_validators[parsedName].IsTrue(x);
            }

            return x => _validators[parsedName].IsTrue(x);
        }

    }
}
