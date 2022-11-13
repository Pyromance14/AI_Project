using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    private int heapIndex;

    public Node parent;

    //Assigns walkable and worldPosition values / Helps Node Keep track of its position.
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;

        gridX = _gridX;
        gridY = _gridY;
    }
    
    //Allows us to get fCost by adding gCost and hCost
    public int fCost
    {
        get { return gCost + hCost; }

    }

    //
    public int HeapIndex
    {
        get { return heapIndex; }

        set { heapIndex = value; }
    }

    //
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
