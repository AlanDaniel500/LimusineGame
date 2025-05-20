using System.Collections.Generic;
using UnityEngine;

public static class QuickSortUtility
{
    public static void QuickSortByDistance(List<GameObject> list, Transform referencePoint)
    {
        QuickSort(list, 0, list.Count - 1, referencePoint);
    }

    private static void QuickSort(List<GameObject> list, int low, int high, Transform reference)
    {
        if (low < high)
        {
            int pi = Partition(list, low, high, reference);
            QuickSort(list, low, pi - 1, reference);
            QuickSort(list, pi + 1, high, reference);
        }
    }

    private static int Partition(List<GameObject> list, int low, int high, Transform reference)
    {
        GameObject pivot = list[high];
        float pivotDistance = Vector2.Distance(pivot.transform.position, reference.position);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            float currentDistance = Vector2.Distance(list[j].transform.position, reference.position);
            if (currentDistance <= pivotDistance)
            {
                i++;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        (list[i + 1], list[high]) = (list[high], list[i + 1]);
        return i + 1;
    }
}

