using System;
using System.Collections.Generic;
using WorkflowModule.Models;

namespace WorkflowModule.Exceptions
{
    public class EventValidationException : Exception
    {
        public IEnumerable<ValidationError> ValidationErrors { get; private set; }

        public EventValidationException(IEnumerable<ValidationError> errors)
        {
            ValidationErrors = errors;
        }
    }
}
