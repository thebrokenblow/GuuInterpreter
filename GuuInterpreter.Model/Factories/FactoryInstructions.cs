using GuuInterpreter.Model.Common;
using GuuInterpreter.Model.DTOs;
using GuuInterpreter.Model.Instructions.Interfaces;
using GuuInterpreter.Model.Instructions.StepInfoInstruction;
using GuuInterpreter.Model.Instructions.StepOverInstruction;

namespace GuuInterpreter.Model.Factories;

public class FactoryInstructions(
    Dictionary<string, Queue<InstructionDto>> queueInstructionsByNameProcedure, 
    Stack<Queue<InstructionDto>> stackInstructionQueues, 
    Stack<InstructionDto> stackRecentProcedureCalls, 
    Stack<string> stackTrace,
    IList<string> printValues,
    VariableRepository variableRepository,
    InterpreterGuu interpreterGuu)
{
    public Dictionary<string, IGuuInstruction> CreateInstructionsStepInfo()
    {
        var executorStepInfoByNameInstruction = new Dictionary<string, IGuuInstruction>()
        {
            { 
                InstructionDictionary.ProcedureCall, new StepInfoCallInstruction(
                                                            queueInstructionsByNameProcedure, 
                                                            stackInstructionQueues, 
                                                            stackRecentProcedureCalls,
                                                            stackTrace,
                                                            interpreterGuu) 
            },
            { InstructionDictionary.PrintVariable, new PrintStepInfoGuuInstruction(variableRepository, printValues) },
            { InstructionDictionary.SetValueVariable, new SetStepInfoGuuInstruction(variableRepository) }
        };

        return executorStepInfoByNameInstruction;
    }

    public Dictionary<string, IGuuInstruction> CreateInstructionsStepOver()
    {
        var executorStepOverByNameInstruction = new Dictionary<string, IGuuInstruction>()
        {
            { InstructionDictionary.PrintVariable, new PrintStepInfoGuuInstruction(variableRepository, printValues) },
            { InstructionDictionary.SetValueVariable, new SetStepInfoGuuInstruction(variableRepository) }
        };

        executorStepOverByNameInstruction.Add(
            InstructionDictionary.ProcedureCall, 
            new StepOverCallInstruction(queueInstructionsByNameProcedure, executorStepOverByNameInstruction, stackTrace));

        return executorStepOverByNameInstruction;
    }
}