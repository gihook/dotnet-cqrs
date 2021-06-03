using System;
using System.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.StateMachine.Validators;

namespace WorkflowModule.StateMachine
{
    public class WorkflowHandlerFactory
    {
        private readonly IEventStore _eventStore;
        private readonly IWorkflowDefinitionLoader _definitionLoader;
        private readonly ReducerTranslator _reducerTranslator;
        private readonly ValidatorTranslator _validatorTranslator;
        private readonly IWorkflowDefinitionHelper _definitionHelper;

        public WorkflowHandlerFactory(IEventStore eventStore, IWorkflowDefinitionLoader definitionLoader)
        {
            _eventStore = eventStore;
            _definitionLoader = definitionLoader;

            _validatorTranslator = new ValidatorTranslator();
            RegisterPrimitiveConverters();
            RegisterDefaultValidators();

            _definitionHelper = new WorkflowDefinitionHelper(_definitionLoader);
            _reducerTranslator = new ReducerTranslator(_definitionHelper);
        }

        private void RegisterPrimitiveConverters()
        {
            foreach (var converterType in GetAllTypes<ITypeConverter>())
            {
                var name = converterType.Name.Replace("Converter", "").ToLower();
                var instance = (ITypeConverter)(Activator.CreateInstance(converterType));
                _validatorTranslator.RegisterConverter(name, instance);
            }
        }

        private Type[] GetAllTypes<T>()
        {
            var types = typeof(WorkflowHandlerFactory).Assembly.GetTypes();
            var converters = types.Where(t =>
            {
                var interfaceType = typeof(T);
                return interfaceType.IsAssignableFrom(t) && !t.IsInterface;
            });

            return converters.ToArray();
        }

        private void RegisterDefaultValidators()
        {
            foreach (var validatorType in GetAllTypes<IInputValidator>())
            {
                var name = validatorType.Name;
                var instance = (IInputValidator)(Activator.CreateInstance(validatorType));
                _validatorTranslator.RegisterValidator(name, instance);
            }
        }

        public void RegisterReducer(string name, IEventReducer reducer)
        {
            _reducerTranslator.RegisterReducer(name, reducer);
        }

        public void RegisterValidator(string name, IInputValidator validator)
        {
            _validatorTranslator.RegisterValidator(name, validator);
        }

        public WorkflowHandler CreateWorkflowHandler()
        {
            var parameterTranslator = new ParameterTranslator();

            var stateChanger = new StateChanger(
                _definitionHelper,
                _validatorTranslator,
                parameterTranslator);

            var stateCalculator = new StateCalculator(_eventStore, _reducerTranslator, stateChanger);

            var eventValidationExecutor = new EventValidationExecutor(
                _definitionHelper,
                _validatorTranslator,
                parameterTranslator);

            var workflowHandler = new WorkflowHandler(stateCalculator, eventValidationExecutor);

            return workflowHandler;
        }
    }
}
