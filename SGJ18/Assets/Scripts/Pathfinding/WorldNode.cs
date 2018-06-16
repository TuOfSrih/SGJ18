using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode : MonoBehaviour {
    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public bool walkable;
    public List<WorldNode> neighbors;
    public WorldNode parent;

    public int Cost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
