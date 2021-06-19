using System;
using System.Collections.Generic;
using System.Linq;
using SencanUtils.Utils.Extensions;

namespace SencanUtils
{
    public static class SUtils
    {
        public static IEnumerable<T> Sort<T>(IEnumerable<T> list) where T : IComparable<T>
        {
            return MergeSort(list.ToList());
        }

        private static List<T> MergeSort<T>(List<T> unsorted) where T : IComparable<T>
        {
            if (unsorted.Count <= 1)
                return unsorted;

            List<T> left = new List<T>();
            List<T> right = new List<T>();

            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++) 
            {
                left.Add(unsorted[i]);
            }
            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            
            return Merge(left, right);
        }

        private static List<T> Merge<T>(List<T> left, List<T> right) where T : IComparable<T>
        {
            List<T> result = new List<T>();

            while(left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().CompareTo(right.First()) == 0 || left.First().CompareTo(right.First()) == -1)  
                    {
                        result.Add(left.First());
                        left.Remove(left.First());    
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if(left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());
                    right.Remove(right.First());    
                }    
            }
            return result;
        }
    }
}
