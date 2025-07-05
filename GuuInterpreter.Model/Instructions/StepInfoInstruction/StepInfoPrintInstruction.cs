using GuuInterpreter.Model.Common;
using GuuInterpreter.Model.Extensions;
using GuuInterpreter.Model.Instructions.Interfaces;

namespace GuuInterpreter.Model.Instructions.StepInfoInstruction;

public class PrintStepInfoGuuInstruction(
    VariableRepository variableStorage, 
    IList<string> printValues) : IGuuInstruction
{
    public void Execute(string[] argumentsInstruction)
    {
        var variableName = argumentsInstruction.Second();

        var value = variableStorage.GetValue(variableName);
        printValues.Add(value.ToString());
    }
}