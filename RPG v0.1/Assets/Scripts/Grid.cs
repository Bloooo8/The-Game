using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeR;
    public bool onlyPath;
    public float minHeight=1;
    Node[,] grid;

    float nodeD;
    int gridY, gridX;

    private void Awake()
    {
        nodeD = nodeR * 2;
        gridX = Mathf.RoundToInt(gridWorldSize.x / nodeD);
        gridY = Mathf.RoundToInt(gridWorldSize.y / nodeD);
        CreateGrid();
        onlyPath = true;

    }

   

    public int MaxSize
    {
        get{ return gridX*gridY; }
    }


    public void CheckWalkability()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                
                bool _walkable = !(Physics.CheckCapsule(grid[x, y].worldPosition, new Vector3(grid[x, y].worldPosition.x, minHeight, grid[x, y].worldPosition.z), nodeR, unwalkableMask));
                grid[x, y].walkable = _walkable;
            }
        }

    }
    void CreateGrid()
    {
        grid = new Node[gridX, gridY];
        Vector3 worldOrigin = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 worldPoint = worldOrigin + Vector3.right * (x * nodeD + nodeR) + Vector3.forward * (y * nodeD + nodeR);
                worldPoint.y =  Terrain.activeTerrain.SampleHeight(worldPoint);
                bool walkable = !(Physics.CheckCapsule(worldPoint,new Vector3(worldPoint.x,minHeight,worldPoint.z) ,nodeR,unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }



    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridXIndex + x;
                int checkY = node.gridYIndex + y;

                if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x+gridWorldSize.x/2)/gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x =Mathf.RoundToInt((gridX - 1) * percentX);
        int y = Mathf.RoundToInt((gridY - 1) * percentY);
        return grid[x, y];

    }
    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (onlyPath)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                   
                            
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeD - .1f));
                }
            }

        }
        else
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeD - .1f));
            }
        }
    }
}
