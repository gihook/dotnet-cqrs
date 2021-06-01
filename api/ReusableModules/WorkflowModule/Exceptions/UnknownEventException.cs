using System;

namespace WorkflowModule.Exceptions
{
    public class UnknownEventException : Exception
    {
        public string EventName { get; set; }

        public UnknownEventException(string eventName)
        {
            EventName = eventName;
        }
    }
}

