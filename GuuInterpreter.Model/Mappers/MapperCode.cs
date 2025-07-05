using GuuInterpreter.Model.Common;
using GuuInterpreter.Model.DTOs;
using GuuInterpreter.Model.Extensions;

namespace GuuInterpreter.Model.Mappers;

public static class MapperCode
{
    public static Dictionary<string, Queue<InstructionDto>> MapProcedures(string code)
    {
        var instructions = MapInstruction(code);

        var instructionByNameProcedure = new Dictionary<string, Queue<InstructionDto>>();
        var currentProcedure = string.Empty;

        foreach (var instruction in instructions)
        {
            var instructionLines = instruction.TextInstruction.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (instruction.TextInstruction.Contains(InstructionDictionary.ProcedureAnnouncement))
            {
                currentProcedure = instructionLines.Second();
                instructionByNameProcedure[currentProcedure] = [];
            }

            instructionByNameProcedure[currentProcedure].Enqueue(instruction);
        }

        return instructionByNameProcedure;
    }

    private static List<InstructionDto> MapInstruction(string code)
    {
        var instructions = new List<InstructionDto>();
        var instructionsLines = code.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);

        for (var i = 0; i < instructionsLines.Length; i++)
        {
            var instructionLine = instructionsLines[i].Trim();
            if (instructionLine == string.Empty)
            {
                continue;
            }
            
            var instruction = new InstructionDto(instructionLine, i + 1);
            instructions.Add(instruction);
        }

        return instructions;
    }
}