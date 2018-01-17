using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AStar : MonoBehaviour
{

   
    Grid grid;
    PathManager requestManager;
    void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathManager>();
    }

   /* public void Update()
    {
        grid.CheckWalkability();
    }*/



    public void StartLookPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(LookForPath(startPos, targetPos));
    }


    IEnumerator LookForPath(Vector3 startPos, Vector3 targetPos)
    {


        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet.RemoveFirst();
                closedSet.Add(node);

                if (node == targetNode)
                {

                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour) + neighbour.heightPenalty;
                    if (newCostToNeighbour <= neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.heuristicCost = GetDistance(neighbour, targetNode);
                        neighbour.heightPenalty =Mathf.Abs(Mathf.RoundToInt(neighbour.worldPosition.y - node.worldPosition.y)*50) ;
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
           
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = ReversePath(startNode, targetNode);
        }
        requestManager.FinishedLooking(waypoints, pathSuccess);
    }

    Vector3[] ReversePath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplePath(path);
        Array.Reverse(waypoints);

        return waypoints;

    }

    Vector3[] SimplePath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridXIndex - path[i].gridXIndex, path[i - 1].gridYIndex - path[i].gridYIndex);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridXIndex - nodeB.gridXIndex);
        int dstY = Mathf.Abs(nodeA.gridYIndex - nodeB.gridYIndex);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}