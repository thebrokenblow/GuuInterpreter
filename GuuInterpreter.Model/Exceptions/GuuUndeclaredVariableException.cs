namespace GuuInterpreter.Model.Exceptions;

public class GuuUndeclaredVariableException : Exception
{
    public string VariableName { get; }

    public GuuUndeclaredVariableException(string variableName)
        : base($"Variable '{variableName}' has not been declared.")
    {
        VariableName = variableName;
    }
}