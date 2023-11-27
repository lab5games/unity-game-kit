using System;
using System.Collections.Generic;

namespace Lab5Games
{
    public static class ListExtension 
    {
        public static T First<T>(this IList<T> inList)
        {
            return inList[0];
        }

        public static T Last<T>(this IList<T> inList)
        {
            return inList[inList.Count - 1];
        }

        public static List<string> GetStringList<T>(this IList<T> inList)
        {
            List<string> result = new List<string>();

            foreach(var item in inList)
            {
                result.Add(item.IsNull() ? "NULL" : item.ToString());
            }

            return result;
        }

        public static List<T> Clone<T>(this IList<T> inList)
        {
            List<T> result = new List<T>();

            foreach(var item in inList)
            {
                result.Add(item);
            }

            return result;
        }

        public static bool ContainsNull<T>(this IList<T> inList)
        {
            foreach(var item in inList)
            {
                if (item.IsNull())
                    return true;
            }

            return false;
        }

        public static int RemoveNulls<T>(this IList<T> inList)
        {
            int cnt = 0;

            for(int i=inList.Count-1; i>=0; i--)
            {
                if (inList[i].IsNull())
                {
                    ++cnt;
                    inList.RemoveAt(i);
                }
            }

            return cnt;
        }

        public static bool AddUnique<T>(this IList<T> inList, T added)
        {
            if (added.IsNull())
                return false;

            if (inList.Contains(added))
                return false;

            inList.Add(added);
            return true;
        }

        public static List<T> Distinct<T>(this IList<T> inList)
        {
            List<T> result = new List<T>();

            foreach(var item in inList)
            {
                result.AddUnique(item);
            }

            return result;
        }

        public static List<T> Reverse<T>(this IList<T> inList)
        {
            int start = 0;
            int end = inList.Count-1;
            List<T> result = inList.Clone();

            while(end < start)
            {
                T temp = result[start];
                result[start] = result[end];
                result[end] = temp;

                start++;
                end--;
            }

            return result;
        }

        public static List<T> Shuffle<T>(this IList<T> inList)
        {
            int n = inList.Count;
            List<T> result = inList.Clone();

            while(n > 1)
            {
                int rIndex = UnityEngine.Random.Range(0, n);
                T rItem = result[rIndex];
                result[rIndex] = result[n - 1];
                result[n - 1] = rItem;
            }

            return result;
        }

        public static void ForEach<T>(this IList<T> inList, Action<T> action)
        {
            foreach(var item in inList)
            {
                action.Invoke(item);
            }
        }

        public static void ForEachWhere<T>(this IList<T> inList, Predicate<T> match, Action<T> action)
        {
            foreach (var item in inList)
            {
                if (match.Invoke(item))
                {
                    action.Invoke(item);
                }
            }
        }
    }
}
