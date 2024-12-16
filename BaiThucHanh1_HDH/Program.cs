using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class MultiThreadedMergeSort
{
    static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = rand.Next(int.MinValue, int.MaxValue);
        }
        return array;
    }

    static void MergeSort(int[] array)
    {
        if (array.Length <= 1) return;

        int mid = array.Length / 2;
        int[] left = array.Take(mid).ToArray();
        int[] right = array.Skip(mid).ToArray();

        MergeSort(left);
        MergeSort(right);

        Merge(array, left, right);
    }

    static void Merge(int[] array, int[] left, int[] right)
    {
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                array[k++] = left[i++];
            }
            else
            {
                array[k++] = right[j++];
            }
        }

        while (i < left.Length)
        {
            array[k++] = left[i++];
        }

        while (j < right.Length)
        {
            array[k++] = right[j++];
        }
    }

    static void ParallelMergeSort(int[] array, int threadCount)
    {
        if (threadCount <= 1 || array.Length <= 1)
        {
            MergeSort(array);
            return;
        }

        int mid = array.Length / 2;
        int[] left = array.Take(mid).ToArray();
        int[] right = array.Skip(mid).ToArray();

        Thread leftThread = new Thread(() => ParallelMergeSort(left, threadCount / 2));
        Thread rightThread = new Thread(() => ParallelMergeSort(right, threadCount / 2));

        leftThread.Start();
        rightThread.Start();

        leftThread.Join();
        rightThread.Join();

        Merge(array, left, right);
    }

    static void Main(string[] args)
    {
        const int arraySize = 1000000000;
        int processorCount = Environment.ProcessorCount;

        Console.WriteLine($"Tao mang voi {arraySize} phan tu...");
        int[] array = GenerateRandomArray(arraySize);
        int[] arrayCopy = new int[array.Length];
        Array.Copy(array, arrayCopy, array.Length);

        Console.WriteLine($"Sap xep bang mot luong...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        MergeSort(array);
        stopwatch.Stop();
        Console.WriteLine($"Sap xep mot luong hoan thanh trong {stopwatch.ElapsedMilliseconds} ms.");

        Console.WriteLine($"Sap xep bang {processorCount} luong...");
        stopwatch.Restart();
        ParallelMergeSort(arrayCopy, processorCount);
        stopwatch.Stop();
        Console.WriteLine($"Sap xep da luong hoan thanh trong {stopwatch.ElapsedMilliseconds} ms.");
    }
}
