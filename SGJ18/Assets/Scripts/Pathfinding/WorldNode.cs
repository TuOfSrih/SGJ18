using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode : MonoBehaviour, IHeapItem<WorldNode> {
    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public bool walkable;
    public List<WorldNode> neighbors;
    public WorldNode parent;

    private int heapIndex;

    public int Cost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(WorldNode b)
    {
        int compare = Cost.CompareTo(b.Cost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(b.hCost);
        }
        return -compare;
    }
}
