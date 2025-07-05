using GuuInterpreter.Model.DTOs;
using GuuInterpreter.Model.Extensions;
using GuuInterpreter.Model.Instructions.Interfaces;

namespace GuuInterpreter.Model.Instructions.StepInfoInstruction;

public class StepInfoCallInstruction(
    Dictionary<string, Queue<InstructionDto>> queueInstructionsByNameProcedure, 
    Stack<Queue<InstructionDto>> stackInstructionQueues, 
    Stack<InstructionDto> stackRecentProcedureCalls,
    Stack<string> stackTrace,
    InterpreterGuu interpreterGuu) : IGuuInstruction
{
    public void Execute(string[] argumentsInstruction)
    {
        stackRecentProcedureCalls.Push(interpreterGuu.CurrentInstruction!);

        var nameProcedure = argumentsInstruction.Second();
        stackTrace.Push(nameProcedure);
        var queueInstructions = queueInstructionsByNameProcedure[nameProcedure];

        var copyQueueInstructions = queueInstructions.GetCopy();
        stackInstructionQueues.Push(copyQueueInstructions);
    }
}