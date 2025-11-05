using System;
using System.Collections.Generic;

namespace BingoMAUI.Helpers
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new();

        // TODO: Can be removed when upgrading to .NET 10 or later
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var list = new List<T>(source);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;
        }
    }
}