using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public CreateNodesFromTilemap map;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    List<WorldNode> FindPath(WorldNode start, WorldNode end)
    {
        BinaryHeap<WorldNode> openSet = new BinaryHeap<WorldNode>(map.gridBoundX * map.gridBoundY);
        HashSet<WorldNode> closeSet = new HashSet<WorldNode>();
        openSet.Add(start);
        while(openSet.Count > 0)
        {
            WorldNode currentNode = openSet.RemoveFirst();
            
            closeSet.Add(currentNode);

            if(currentNode == end)
            {
                return RetracePath(start, end);
            }

            foreach(WorldNode n in currentNode.neighbors)
            {
                if(n.walkable && !closeSet.Contains(n))
                {
                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, n);
                    if(newMovementCostToNeighbor < n.gCost || !openSet.Contains(n))
                    {
                        n.gCost = newMovementCostToNeighbor;
                        n.hCost = GetDistance(n, end);
                        n.parent = currentNode;

                        if (!openSet.Contains(n))
                        {
                            openSet.Add(n);
                            openSet.UpdateItem(n);
                        }
                    }
                }
            }
        }
        return new List<WorldNode>();
    }

    List<WorldNode> RetracePath(WorldNode start, WorldNode end)
    {
        List<WorldNode> path = new List<WorldNode>();
        WorldNode currentNode = end;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
    
    int GetDistance(WorldNode a, WorldNode b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
