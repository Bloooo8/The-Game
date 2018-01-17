using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node :IHeapItem<Node> {

    public bool walkable;
    public Vector3 worldPosition;
    public int gridXIndex;
    public int gridYIndex;

    public int gCost;
    public int heuristicCost;
    public int heightPenalty;
    public Node parent;

    int heapIndex;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
        gridXIndex = gridX;
        gridYIndex = gridY;
    }

    public int fullCost
    {
        get
        {
            return gCost + heuristicCost + heightPenalty;
        }
    }
     public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fullCost.CompareTo(nodeToCompare.fullCost);
        if (compare == 0)
        {
            compare = heuristicCost.CompareTo(nodeToCompare.heuristicCost);
        }
        return -compare;
    }
}
