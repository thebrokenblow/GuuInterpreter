using GuuInterpreter.Model.DTOs;
using GuuInterpreter.Model.Exceptions;
using GuuInterpreter.Model.Extensions;
using GuuInterpreter.Model.Instructions.Interfaces;
using GuuInterpreter.Model.Mappers;

namespace GuuInterpreter.Model.Instructions.StepOverInstruction;

public class StepOverCallInstruction(
    Dictionary<string, Queue<InstructionDto>> queueInstructionsByNameProcedure,
    Dictionary<string, IGuuInstruction> instructionSetOverByNameInstruction,
    Stack<string> stackTrace) : IGuuInstruction
{
    private int _numberRecursiveCalls;
    private const int MaxNumberRecursiveCalls = 1000;
    public void Execute(string[] argumentsInstruction)
    {
        var nameProcedure = argumentsInstruction.Second();
        stackTrace.Push(nameProcedure);
        var queueInstructions = queueInstructionsByNameProcedure[nameProcedure];
        var copyQueueInstructions = queueInstructions.GetCopy();

        while (copyQueueInstructions.Count != 0)
        {
            _numberRecursiveCalls++;

            if (_numberRecursiveCalls == MaxNumberRecursiveCalls)
            {
                throw new GuuStackOverflowException("StackOverflow");
            }

            var instructionDto = copyQueueInstructions.Dequeue();
            argumentsInstruction = MapperInstruction.Map(instructionDto.TextInstruction);

            var nameInstruction = argumentsInstruction.First();

            if (instructionSetOverByNameInstruction.TryGetValue(nameInstruction, out var instruction))
            {
                instruction.Execute(argumentsInstruction);
            }
        }
    }
}