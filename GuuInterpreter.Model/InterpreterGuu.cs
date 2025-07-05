using GuuInterpreter.Model.Common;
using GuuInterpreter.Model.DTOs;
using GuuInterpreter.Model.Factories;
using GuuInterpreter.Model.Instructions.Interfaces;
using GuuInterpreter.Model.Mappers;

namespace GuuInterpreter.Model;

public class InterpreterGuu
{
    private readonly Stack<Queue<InstructionDto>> _stackInstructionQueues = [];
    private readonly Stack<InstructionDto> _stackRecentProcedureCalls = [];
    private readonly Stack<string> _stackTrace = [];

    private readonly Dictionary<string, IGuuInstruction> _instructionStepInfoByNameInstruction;
    private readonly Dictionary<string, IGuuInstruction> _instructionStepOverByNameInstruction;

    private readonly VariableRepository _variableRepository;
    public int NumberInstruction { get; private set; }
    public InstructionDto? CurrentInstruction { get; private set; }
    public IReadOnlyDictionary<string, int> ValueByNameVariable => _variableRepository.ValueByNameVariable;
    public IReadOnlyCollection<string> StackTrace => _stackTrace;

    public bool IsProgramFinish
    {
        get
        {
            var queueInstructions = _stackInstructionQueues.Peek();

            return _stackInstructionQueues.Count == 1 && queueInstructions.Count == 0;
        }
    }

    public InterpreterGuu(string code, IList<string> printValues)
    {
        var queueInstructionsByNameProcedure = MapperCode.MapProcedures(code);
        var instruction = queueInstructionsByNameProcedure[InstructionDictionary.NameMainProcedure];

        _stackTrace.Push(InstructionDictionary.NameMainProcedure);
        _stackInstructionQueues.Push(instruction);

        _variableRepository = new VariableRepository();
        var factoryInstruction = new FactoryInstructions(
            queueInstructionsByNameProcedure, 
            _stackInstructionQueues, 
            _stackRecentProcedureCalls,
            _stackTrace,
            printValues,
            _variableRepository,
            this);

        _instructionStepInfoByNameInstruction = factoryInstruction.CreateInstructionsStepInfo();
        _instructionStepOverByNameInstruction = factoryInstruction.CreateInstructionsStepOver();
    }

    public void Execute(TypeCodeExecution typeExecute)
    {
        if (IsProgramFinish)
        {
            return;
        }

        var queueInstructions = _stackInstructionQueues.Peek();
        if (queueInstructions.Count == 0 && _stackRecentProcedureCalls.Count != 0)
        {
            RestoreProcedureCallPosition();
            return;
        }

        CurrentInstruction = queueInstructions.Dequeue();
        ExecuteInstruction(typeExecute);
    }

    private void ExecuteInstruction(TypeCodeExecution typeExecute)
    {
        var argumentsInstruction = MapperInstruction.Map(CurrentInstruction!.TextInstruction);
        var nameInstruction = argumentsInstruction.First();

        if (typeExecute == TypeCodeExecution.StepInfo)
        {
            if (_instructionStepInfoByNameInstruction.TryGetValue(nameInstruction, out var guuInstruction))
            {
                guuInstruction.Execute(argumentsInstruction);
            }
        }
        else
        {
            if (_instructionStepOverByNameInstruction.TryGetValue(nameInstruction, out var guuInstruction))
            {
                guuInstruction.Execute(argumentsInstruction);
            }
        }

        NumberInstruction = CurrentInstruction.NumberInstruction;
    }

    private void RestoreProcedureCallPosition()
    {
        _stackInstructionQueues.Pop();

        var recentProcedureCalls = _stackRecentProcedureCalls.Pop();
        NumberInstruction = recentProcedureCalls.NumberInstruction;
    }
}