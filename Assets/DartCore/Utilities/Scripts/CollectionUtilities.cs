using System.Collections.Generic;

namespace DartCore.Utilities
{
    public class CollectionUtilities
    {
        public static T[] RemoveDuplicates<T>(T[] array)
        {
            var tempList = new List<T>();
            foreach (var item in array)
            {
                if (!tempList.Contains(item))
                    tempList.Add(item);
            }

            return tempList.ToArray();
        }
        
        public static List<T> RemoveDuplicates<T>(List<T> list)
        {
            var newList = new List<T>();
            foreach (var item in list)
            {
                if (!newList.Contains(item))
                    newList.Add(item);
            }

            return newList;
        }
    }
}