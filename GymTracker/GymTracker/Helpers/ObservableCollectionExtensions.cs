using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GymTracker.Helpers
{
    public static class ObservableCollectionExtensionN
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, List<T> list)
        {
            foreach (var item in list)
            {
                collection.Add(item);
            }
        }
    }
}
