using System;
using WorkflowModule.Models;

namespace WorkflowModule.Exceptions
{
    public class NonExistingTransitionException : Exception
    {
        public EventDataWithState EventDataWithState { get; set; }

        public NonExistingTransitionException(EventDataWithState eventDataWithState)
        {
            EventDataWithState = eventDataWithState;
        }
    }
}

