using GuuInterpreter.Model.Exceptions;

namespace GuuInterpreter.Model.Common;

public class VariableRepository
{
    private readonly Dictionary<string, int> _valueByNameVariable = [];
    public IReadOnlyDictionary<string, int> ValueByNameVariable => _valueByNameVariable;

    public void SetValue(string nameVariable, int value)
    {
        _valueByNameVariable[nameVariable] = value;
    }

    public int GetValue(string nameVariable)
    {
        if (!_valueByNameVariable.TryGetValue(nameVariable, out var value))
        {
            throw new GuuUndeclaredVariableException(nameVariable);
        }

        return value;
    }
}