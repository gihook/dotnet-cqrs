using System;
using WorkflowModule.Descriptors;

namespace WorkflowModule.Interfaces
{
    public interface IValidatorTranslator
    {
        bool CanParse(string type, object parameterValue);
        Func<object[], bool> GetValidator(InputValidatorDescriptor descriptor);
    }
}
