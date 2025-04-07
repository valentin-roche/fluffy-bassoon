using System.Collections.Generic;

namespace Grids
{
    public static class ArrayExtensionMethods
    {
        public static List<int> RemoveAllSpecifiedIndicesFromArray(this int[] a, bool[] indicesToRemove)
        {
            List<int> b = new List<int>();
            for (int i = 0; i < indicesToRemove.Length; ++i)
            {
                if (!indicesToRemove[i])
                    b.Add(a[i]);
            }
            return b;
        }
        public static List<bool> Merge(this bool[] a, bool[] b)
        {
            List<bool> res = new List<bool>();
            for (int i = 0; i < b.Length; ++i)
            {
                res.Add(a[i] && b[i]);
            }
            return res;
        }
    }
}