using System;
using System.Collections.Generic;

namespace SchizoQuest.Helpers
{
    public static class ListHelpers
    {
        /// <summary>
        /// Remove an element from the list by swapping it with the last element and removing the last element instead.<br/>
        /// This method is O(1), which is faster than the O(n) of <see cref="List{T}.Remove(T)"/> and <see cref="List{T}.RemoveAt(int)"/>.
        /// </summary>
        /// <remarks>
        /// Because this method swaps elements, it should only be used when the list order does not matter.
        /// </remarks>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="list">List to remove from.</param>
        /// <param name="index">Index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is out of range of the <see cref="List{T}.Count"/>.</exception>
        public static void RemoveSwap<T>(this List<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            list[index] = list[^1];
            list.RemoveAt(list.Count - 1);
        }
    }
}
