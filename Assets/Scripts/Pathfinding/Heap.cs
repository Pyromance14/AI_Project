using System.Collections;
using UnityEngine;
using System;


public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        //Determines maximum size of Heap.
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        //Each item keeps track of its own index within the Heap.
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        //Removes first item from heap.
        T firstItem = items[0];
        currentItemCount--;

        //Takes item at end of heap and places it at first place.
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;


        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem (T item)
    {
        SortUp(item);
    }

    public int Count
    {
        //Gives number of items currently in the heap.
        get { return currentItemCount; }
    }

    public bool Contains (T item)
    {
        //Checks if two items are equal
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;

            int swapIndex = 0;


            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }

                else
                {
                    return;
                }
            }

            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            //Compares Item and sorts by highest priority.
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                //If item has highest priority then parentItem they are both swap.
                Swap (item, parentItem);
            }

            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;

        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex 
    { 
        get; 
        set; 
    }
}
