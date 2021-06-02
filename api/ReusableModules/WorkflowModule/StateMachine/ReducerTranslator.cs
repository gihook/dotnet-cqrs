using System.Collections.Generic;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class ReducerTranslator : IReducerTranslator
    {
        private readonly Dictionary<string, IEventReducer> _reducers;
        private readonly IWorkflowDefinitionHelper _definitionHelper;

        public ReducerTranslator(IWorkflowDefinitionHelper definitionHelper)
        {
            _reducers = new Dictionary<string, IEventReducer>();
            _definitionHelper = definitionHelper;
        }

        public void RegisterReducer(string reducerName, IEventReducer reducer)
        {
            _reducers[reducerName] = reducer;
        }

        public IEventReducer GetReducer(EventPayload payload, string workflowId)
        {
            var eventDescriptor = _definitionHelper.GetEventDescriptor(new EventDataWithState
            {
                EventPayload = payload,
                WorkflowId = workflowId
            });

            var reducerName = eventDescriptor.ReducerDescriptor.Type;

            if (!_reducers.ContainsKey(reducerName)) throw new UnknownReducerException(reducerName);

            return _reducers[reducerName];
        }
    }
}
