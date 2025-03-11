namespace LibraryModule.Helpers
{
    internal static class SortingHelper
    {
        public static void QuickSort<T>(List<T> items, Func<T, T, int> comparer)
        {
            QuickSort(items, 0, items.Count - 1, comparer);
        }

        private static void QuickSort<T>(List<T> items, int lowIndex, int highIndex, Func<T, T, int> comparer)
        {
            if (lowIndex < highIndex)
            {
                int pivotIndex = Partition(items, lowIndex, highIndex, comparer);
                QuickSort(items, lowIndex, pivotIndex - 1, comparer);
                QuickSort(items, pivotIndex + 1, highIndex, comparer);
            }
        }

        private static int Partition<T>(List<T> items, int lowIndex, int highIndex, Func<T, T, int> comparer)
        {
            T pivot = items[highIndex];
            int i = lowIndex - 1;

            for (int j = lowIndex; j < highIndex; j++)
            {
                if (comparer(items[j], pivot) <= 0)
                {
                    i++;
                    (items[i], items[j]) = (items[j], items[i]);
                }
            }

            (items[i + 1], items[highIndex]) = (items[highIndex], items[i + 1]);
            return i + 1;
        }
    }
}
