namespace WorkflowModule.Interfaces
{
    public interface IParameterTranslator
    {
        object GetParameterValue(string encodedParameter);
    }
}
