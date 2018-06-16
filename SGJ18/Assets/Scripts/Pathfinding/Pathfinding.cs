using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public CreateNodesFromTilemap map;

    private Stack<WorldNode> path;

    private WorldNode nextNode;

    public WorldNode currentTile;

    public GameObject player;

    public float speed = 5;

    // TODO delete when unnecessary!!
    public int endX;
    public int endY;

    IEnumerator trail = null;

	// Use this for initialization
	void Start () {
        path = new Stack<WorldNode>();
        currentTile = map.GetNodeAt(transform.position);
        Debug.Log("[" + currentTile.gridX + "," + currentTile.gridY + "]");
        StartCoroutine(TrailPlayer());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TrailPlayer()
    {
        while (true)
        {
            WorldNode playerTile = map.GetNodeAt(player.transform.position);
            if(trail != null)
                StopCoroutine(trail);
            trail = WalkPath(playerTile);
            StartCoroutine(trail);
            yield return new WaitForSeconds(0.7f);
        }
    }

    IEnumerator WalkPath(WorldNode dst)
    {
        //Debug.Log("Started Walking.");
        FindPath(currentTile, dst);
        //Debug.Log("Found Path to [" + dst.gridX + "," + dst.gridY + "]");
        //StartCoroutine(FindPath(currentTile, dst));
        while(true)
        {
            if(transform.position == currentTile.transform.position)
            {
                if(path.Count == 0)
                {
                    yield break;
                }
                currentTile = path.Pop();
            }
            transform.position = Vector3.MoveTowards(transform.position, currentTile.transform.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    

    void FindPath(WorldNode start, WorldNode end)
    {
        Debug.Log("Started Path finding " + end.walkable);
        bool pathSuccess = false;

        //BinaryHeap<WorldNode> openSet = new BinaryHeap<WorldNode>(map.gridBoundX * map.gridBoundY);
        List<WorldNode> openSet = new List<WorldNode>();
        HashSet<WorldNode> closeSet = new HashSet<WorldNode>();
        openSet.Add(start);
        Debug.Log(openSet.Count);
        while(openSet.Count > 0)
        {
            //WorldNode currentNode = openSet.RemoveFirst();
            WorldNode currentNode = openSet[0];
            for(int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].Cost < currentNode.Cost || openSet[i].Cost == currentNode.Cost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            
            
            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if(currentNode == end)
            {
                pathSuccess = true;
                break;
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
                            //openSet.UpdateItem(n);
                        }
                    }
                }
            }
            //yield return new WaitForSeconds(1.2f);
        }
        if (pathSuccess)
        {
            RetracePath(start, end);
        }
    }

    void RetracePath(WorldNode start, WorldNode end)
    {
        path = new Stack<WorldNode>();
        WorldNode currentNode = end;

        while(currentNode != start)
        {
            path.Push(currentNode);
            currentNode = currentNode.parent;
        }
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
