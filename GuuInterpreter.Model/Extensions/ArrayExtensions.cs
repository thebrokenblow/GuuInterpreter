namespace GuuInterpreter.Model.Extensions;

public static class ArrayExtensions
{
    public static T Second<T>(this T[] array)
    {
        if (array.Length < 2)
        {
            throw new InvalidOperationException("Array must have at least two elements.");
        }

        return array[1];
    }

    public static T Third<T>(this T[] array)
    {
        if (array.Length < 3)
        {
            throw new InvalidOperationException("Array must have at least three elements.");
        }

        return array[2];
    }
}