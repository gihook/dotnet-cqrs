using System;

namespace WorkflowModule.Exceptions
{
    public class UnknownReducerException : Exception
    {
        public string ReducerName { get; set; }

        public UnknownReducerException(string reducerName)
        {
            ReducerName = reducerName;
        }
    }
}
