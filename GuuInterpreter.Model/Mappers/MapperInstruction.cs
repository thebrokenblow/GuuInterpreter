namespace GuuInterpreter.Model.Mappers;

public static class MapperInstruction
{
    public static string[] Map(string instruction)
    {
        return instruction.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    }
}