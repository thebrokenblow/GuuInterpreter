namespace GuuInterpreter.Model.Extensions;

public static class QueueExtensions
{
    public static Queue<T> GetCopy<T>(this Queue<T> originalQueue)
    {
        return new Queue<T>(originalQueue);
    }
}