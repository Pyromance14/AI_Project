using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        //Finds the position of the player drawing the path between them and the AI (enemy/seeker)
            FindPath(seeker.position, target.position);
    }


    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        //Converts world position into Nodes
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);


        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            //Finds Node within OpenSet with the lowest fCost
            Node currentNode = openSet.RemoveFirst();   
            closedSet.Add(currentNode);

            //Path is Found
            if(currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");

                RetracePath(startNode, targetNode);
                return;
            }

            //Loop through all Neighbours
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                //if neighbour isn't walkable or is the closed list, skip to next neighbour.
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }


                //Checks if new path to neighbour is shorter than the old path.
                //Also checks if neighbour isn't in open list.
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //Sets the f_cost (by calculating g_cost & h_cost)
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    //and parent of neighbour to the currentNode.
                    neighbour.parent = currentNode;

                    //will add neighbour to open if they aren't there.
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }


    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        //Retraces path until it reaches the starting Node. ( Helps create the Path )
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //Makes path move in the correct direction.
        path.Reverse();

        grid.path = path;
    }

    //First uses x-axis to determine how many nodes far way Ai (seeker/enemy) is from the player.
    //Then uses to y-axis to determine distance from player.
    //Takes lowest number to determine how far diagonally/vertically AI will need to be in order
    //to be in line with Target.
    //Finally determines the number of horizontal moves are needed by subracting lower number by
    // the higher number.
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
            return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
