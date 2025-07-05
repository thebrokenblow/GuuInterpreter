using GuuInterpreter.Model.Common;
using GuuInterpreter.Model.Extensions;
using GuuInterpreter.Model.Instructions.Interfaces;

namespace GuuInterpreter.Model.Instructions.StepInfoInstruction;

public class SetStepInfoGuuInstruction(VariableRepository variableStorage) : IGuuInstruction
{
    public void Execute(string[] argumentsInstruction)
    {
        var variableName = argumentsInstruction.Second();
        var stringValue = argumentsInstruction.Third();

        var value = int.Parse(stringValue);

        variableStorage.SetValue(variableName, value);
    }
}