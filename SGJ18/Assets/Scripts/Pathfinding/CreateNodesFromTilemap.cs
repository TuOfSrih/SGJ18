using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateNodesFromTilemap : MonoBehaviour {
    public Grid gridBase;
    public Tilemap floor; // Maybe unnecessary
    public List<Tilemap> obstacleLayers;
    public GameObject nodePrefab;

    public int scanStartX, scanStartY, scanEndX, scanEndY;

    public int gridBoundX, gridBoundY;

    public List<GameObject> unsortedNodes;
    public GameObject[,] nodes;

    void CreateNodes()
    {
        int gridX = 0;
        int gridY = 0;

        bool foundTileOnLastPass = false;

        for(int x = scanStartX; x < scanEndX; x++)
        {
            for(int y = scanStartY; y < scanEndY; y++)
            {
                TileBase tb = floor.GetTile(new Vector3Int(x, y, 0));

                if(tb != null)
                {
                    bool foundObstacle = false;
                    foreach(Tilemap t in obstacleLayers)
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(x, y, 0));

                        Debug.Log(tb2);
                        if (tb2 != null)
                        {
                            Debug.Log("Found obstactle");
                            foundObstacle = true;
                        }

                        if(unwalkableNodeBorder > 0)
                        {
                            List<TileBase> neighbors = getNeighborTiles(x, y, t);
                            foreach(TileBase tn in neighbors){
                                if(tn != null)
                                {
                                    foundObstacle = true;
                                }
                            }   
                        }
                    }
                    GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x,
                                                                                            y + 0.5f + gridBase.transform.position.y, 0),
                                                                                            Quaternion.identity);

                    foundTileOnLastPass = true;

                    WorldNode wn = node.GetComponent<WorldNode>();
                    if(wn != null)
                    {
                        wn.gridX = gridX;
                        wn.gridY = gridY;
                    }
                    

                    unsortedNodes.Add(node);

                    if (!foundObstacle)
                    {                       
                        wn.walkable = true;
                    }
                    else
                    {
                        node.GetComponent<SpriteRenderer>().color = Color.red;

                        wn.walkable = false;
                    }

                    gridY++;

                    if(gridX > gridBoundX)
                    {
                        gridBoundX = gridX;
                    }
                    if(gridY > gridBoundY)
                    {
                        gridBoundY = gridY;
                    }
                }
            }
            if (foundTileOnLastPass)
            {
                gridX++;
                gridY = 0;
                foundTileOnLastPass = false;
            }
        }
        nodes = new GameObject[gridBoundX + 1, gridBoundY + 1];
        foreach (var node in unsortedNodes)
        {
            WorldNode wn = node.GetComponent<WorldNode>();
            nodes[wn.gridX, wn.gridY] = node;
        }

        for(int x = 0; x < gridBoundX; x++)
        {
            for(int y = 0; y < gridBoundY; y++)
            {
                if(nodes[x,y] != null)
                {
                    WorldNode wn = nodes[x, y].GetComponent<WorldNode>();
                    wn.neighbors = GetNeighbors(x, y);
                }
            }
        }

    }

    private WorldNode GetNodeAt(int x, int y)
    {
        if(x < 0 || y < 0 || x >= gridBoundX || y >= gridBoundY)
        {
            return null;
        }

        if(nodes[x, y] != null)
        {
            return nodes[x, y].GetComponent<WorldNode>();
        }
        return null;
    }

    private void AddToList(int x, int y, List<WorldNode> list)
    {
        WorldNode wn = GetNodeAt(x, y);
        if(wn != null)
        {
            list.Add(wn);
        }
    }

    private List<WorldNode> GetNeighbors(int x, int y)
    {
        List<WorldNode> neighbors = new List<WorldNode>();

        AddToList(x - 1, y - 1, neighbors);
        AddToList(x, y - 1, neighbors);
        AddToList(x + 1, y - 1, neighbors);
        AddToList(x - 1, y, neighbors);
        AddToList(x + 1, y, neighbors);
        AddToList(x - 1, y + 1, neighbors);
        AddToList(x, y + 1, neighbors);
        AddToList(x + 1, y + 1, neighbors);

        return neighbors;
    }

    public int unwalkableNodeBorder;
    private List<TileBase> getNeighborTiles(int x, int y, Tilemap t)
    {
        List<TileBase> ret = new List<TileBase>();
        for(int i = x - unwalkableNodeBorder; i < x + unwalkableNodeBorder; i++)
        {
            for(int j = y - unwalkableNodeBorder; j < y + unwalkableNodeBorder; j++)
            {
                TileBase tile = t.GetTile(new Vector3Int(i, j, 0));
                if(tile != null)
                {
                    ret.Add(tile);
                }
            }
        }
        return ret;
    }

    void Awake()
    {
        unsortedNodes = new List<GameObject>();
        CreateNodes();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
