using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    //Helps determine how many nodes can fit onto grid based on NodeRadius.
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        
        //Gives how many Nodes can fit into x Axis of world.
        //Round it to nearest integer to avoid half Nodes.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);

        //Gives how many Nodes can fit into y Axis of world.
        //Round it to nearest integer to avoid half Nodes.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        
        //Helps with finding worlpoint when looping through all positions of the Node.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        //Loop through all positions the Nodes will be in for a collision check.
        //Determines which areas are walkable and which ones aren't.
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }


    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //Searches in a 3x3 block.
        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    //Converts world position to a Node corrdinate to help find the Node the player is on.
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //Converts world position (x, y coordinates )into a percentage.
        // (x) far left = 0, middle = .5 & far right = 1
        // (y) bottom = 0, middle = .5 & top = 1
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //Finds x & y intercepts of 2D grid Array
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    public List<Node> path;

    //Makes the path crossed by the Node visible.
    //AI (seeker/enemy) pathway is highlighted in Magenta.
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }

        else
        {
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.green : Color.blue;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}

