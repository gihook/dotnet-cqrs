using WorkflowModule.Descriptors;

namespace WorkflowModule.Interfaces
{
    public interface IReducerTranslator
    {
        IEventReducer GetReducer(EventDescriptor descriptor);
    }
}
