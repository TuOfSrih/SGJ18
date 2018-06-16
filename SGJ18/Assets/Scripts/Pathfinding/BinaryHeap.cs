using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BinaryHeap<T> where T : IHeapItem<T> {
    private T[] items;
    int itemCount;

    public BinaryHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[itemCount] = item;
        SiftUp(item);
        itemCount++;
    }

    public void UpdateItem(T item)
    {
        SiftUp(item);
    }

    public T RemoveFirst()
    {
        T ret = items[0];
        itemCount--;
        items[0] = items[itemCount];
        items[0].HeapIndex = 0;

        SiftDown(items[0]);

        return ret;
    }

    public int Count
    {
        get
        {
            return itemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void SiftUp(T item)
    {
        int parentHeapIndex = (item.HeapIndex-1)/ 2;

        while (true)
        {
            T parentItem = items[parentHeapIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                return;
            }

            parentHeapIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SiftDown(T item)
    {
        while (true)
        {
            int childLeftIndex = 2 * item.HeapIndex + 1;
            int childRightIndex = 2 * item.HeapIndex + 2;
            int swapIndex = 0;

            if(childLeftIndex < itemCount)
            {
                swapIndex = childLeftIndex;

                if(childRightIndex < itemCount)
                {
                    if (items[childLeftIndex].CompareTo(items[childRightIndex]) < 0) {
                        swapIndex = childRightIndex;
                    }

                    if(item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    private void Swap(T a, T b)
    {
        items[a.HeapIndex] = b;
        items[b.HeapIndex] = a;
        int itemAIndex = a.HeapIndex;
        a.HeapIndex = b.HeapIndex;
        b.HeapIndex = itemAIndex;
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
