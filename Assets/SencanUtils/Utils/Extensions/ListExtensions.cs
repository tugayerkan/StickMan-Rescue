using System.Collections.Generic;
using UnityEngine;

namespace SencanUtils.Utils.Extensions
{
    public static class ListExtensions
    {
        public static T First<T>(this List<T> list) => list[0];
        public static T Last<T>(this List<T> list) => list[list.Count - 1];
        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];
    }
}