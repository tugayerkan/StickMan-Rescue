using UnityEngine;

namespace SencanUtils.Utils.Extensions
{
    public static class ArrayExtensions
    {
        public static T First<T>(this T[] array) => array[0];
        public static T Last<T>(this T[] array) => array[array.Length - 1];
        public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];
    }
}