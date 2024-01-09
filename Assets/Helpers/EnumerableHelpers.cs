using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class EnumerableHelpers
    {
        public static Vector2 Sum(this IEnumerable<Vector2> source)
            => source.Aggregate(Vector2.zero, (current, next) => current + next);
    }
}