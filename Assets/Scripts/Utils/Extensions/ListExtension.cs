using System.Collections.Generic;
using UnityEngine;

namespace Utils.Extensions
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int count = list.Count;
            for (int i = count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}