using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public CreateNodesFromTilemap map;

    private Stack<WorldNode> path;

    public WorldNode currentTile;

    public GameObject player;

    public float speed = 5;

    IEnumerator trail = null;

    public Animator animator;

    public Vector3 DirToPlayer
    {
        get
        {
            return (player.transform.position - transform.position).normalized;
        }
    }

    public int TileDistToPLayer
    {
        get
        {
            return GetDistance(currentTile, map.GetNodeAt(player.transform.position));
        }
    }

	void Start () {
        path = new Stack<WorldNode>();
        currentTile = map.GetNodeAt(transform.position);
	}

    public IEnumerator SearchPlayer()
    {
        while (true)
        {
            if(Random.value > 0.25)
            {
                WorldNode destNode = null;
                while(destNode == null)
                {
                    Vector3 dir = new Vector3(Random.value, Random.value, 0) * 6 - new Vector3(3, 3, 0);
                    destNode = map.GetNodeAt(transform.position + dir);
                    if(destNode != null)
                    {
                        if (!destNode.walkable)
                        {
                            destNode = null;
                        }
                    }
                }

                if (trail != null)
                {
                    StopCoroutine(trail);                  
                }
                trail = WalkPath(destNode);
                StartCoroutine(trail);
            }
            yield return new WaitForSeconds(1.3f);
        }
    }

    public IEnumerator TrailPlayer()
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

    public IEnumerator AbandonPlayer()
    {
        while (true)
        {
            if (Random.value > 0.15)
            {
                WorldNode destNode = null;
                while (destNode == null)
                {
                    Vector3 dir = DirToPlayer * -1.5f;
                    Vector3 deviation = new Vector3(Random.value, Random.value, 0) * 5 - new Vector3(2.5f, 2.5f, 0);
                    dir -= deviation;

                    destNode = map.GetNodeAt(transform.position + dir);
                    if (destNode != null)
                    {
                        if (!destNode.walkable)
                        {
                            destNode = null;
                        }
                    }
                }

                if (trail != null)
                {
                    StopCoroutine(trail);
                }
                trail = WalkPath(destNode);
                StartCoroutine(trail);
            }
            yield return new WaitForSeconds(1.6f);
        }
    }

    IEnumerator WalkPath(WorldNode dst)
    {
        //Debug.Log("Started Walking.");
        path = FindPath(currentTile, dst);
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
            animator.SetBool("isWalking", true);
            Debug.Log("X: " + animator.GetFloat("x"));
            Vector2 vector = ((Vector2) currentTile.transform.position - (Vector2) transform.position).normalized;
            animator.SetFloat("x", vector.x);
            animator.SetFloat("y", vector.y);
            yield return null;
        }
    }

    Stack<WorldNode> FindPath(WorldNode start, WorldNode end)
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
            return RetracePath(start, end);
        }
        return new Stack<WorldNode>();
    }

    Stack<WorldNode> RetracePath(WorldNode start, WorldNode end)
    {
        Stack<WorldNode> ret = new Stack<WorldNode>();
        WorldNode currentNode = end;

        while(currentNode != start)
        {
            ret.Push(currentNode);
            currentNode = currentNode.parent;
        }
        return ret;
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
