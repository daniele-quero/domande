using System.Collections.Generic;
using UnityEngine;

public class DurstenfeldShuffler<T>
{
    public static void shuffle(IList<T> array)
    {
        int l = array.Count;
        for(int i = l-1; i>=0; i--)
        {
            int j = Random.Range(0, i);
            swap(array, i, j);
        }
    }

    private static void swap(IList<T> array, int i, int j) 
    {
        T el = array[i];
        array[i] = array[j];
        array[j] = el;
    }

}
