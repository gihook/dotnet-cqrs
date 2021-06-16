using System;
using System.Linq;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachine
{
    public class StateMachineHandlerFactory
    {
        private readonly IEventStore _eventStore;
        private readonly IWorkflowDefinitionLoader _definitionLoader;
        private readonly ReducerTranslator _reducerTranslator;
        private readonly ValidatorTranslator _validatorTranslator;
        private readonly IWorkflowDefinitionHelper _definitionHelper;

        public StateMachineHandlerFactory(IEventStore eventStore, IWorkflowDefinitionLoader definitionLoader)
        {
            _eventStore = eventStore;
            _definitionLoader = definitionLoader;

            _validatorTranslator = new ValidatorTranslator();
            RegisterPrimitiveConverters();
            RegisterDefaultValidators();

            _definitionHelper = new StateMachineDefinitionHelper(_definitionLoader);
            _reducerTranslator = new ReducerTranslator(_definitionHelper);
        }

        private void RegisterPrimitiveConverters()
        {
            foreach (var converterType in GetAllTypes<StateMachineHandlerFactory, ITypeConverter>())
            {
                var name = converterType.Name.Replace("Converter", "").ToLower();
                var instance = (ITypeConverter)(Activator.CreateInstance(converterType));
                RegisterConverter(name, instance);
            }
        }

        private Type[] GetAllTypes<AnyAssemblyType, InterfaceType>()
        {
            var types = typeof(AnyAssemblyType).Assembly.GetTypes();
            var converters = types.Where(t =>
            {
                var interfaceType = typeof(InterfaceType);
                return interfaceType.IsAssignableFrom(t) && !t.IsInterface;
            });

            return converters.ToArray();
        }

        private void RegisterDefaultValidators()
        {
            foreach (var validatorType in GetAllTypes<StateMachineHandlerFactory, IInputValidator>())
            {
                var name = validatorType.Name;
                var instance = (IInputValidator)(Activator.CreateInstance(validatorType));
                RegisterValidator(name, instance);
            }
        }

        public void RegisterAllReducersFromAssembly<AnyAssemblyType>()
        {
            foreach (var reducerType in GetAllTypes<AnyAssemblyType, IEventReducer>())
            {
                var name = reducerType.Name;
                var instance = (IEventReducer)(Activator.CreateInstance(reducerType));
                RegisterReducer(name, instance);
            }
        }

        public void RegisterConverter(string name, ITypeConverter converter)
        {
            _validatorTranslator.RegisterConverter(name, converter);
        }

        public void RegisterReducer(string name, IEventReducer reducer)
        {
            _reducerTranslator.RegisterReducer(name, reducer);
        }

        public void RegisterValidator(string name, IInputValidator validator)
        {
            _validatorTranslator.RegisterValidator(name, validator);
        }

        public StateMachineHandler CreateStateMachineHandler()
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

            var workflowHandler = new StateMachineHandler(stateCalculator, eventValidationExecutor);

            return workflowHandler;
        }
    }
}
