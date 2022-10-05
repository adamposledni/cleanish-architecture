namespace Onion.Core.Extensions;

public static class IEnumerableExtensions
{
    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
    {
        return new LinkedList<T>(source);
    }
}
