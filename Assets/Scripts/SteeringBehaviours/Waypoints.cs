using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] 
    private float waypointSize = 1f;

    //Draws the Path that that the AI will follow
    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            //The target points of the AI will be coloured Yellow.
            //Size of waypoint can be changed in Game Scene.
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(t.position, waypointSize);
        }

        //The movement path of the AI will be coloured Magenta.
        Gizmos.color = Color.magenta;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            //Draws a line between the first waypoint and the last. 
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        //Connects last child with the first one.
        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        //When method is called and waypoint doesn't exist object is moved to first waypoint.
        if(currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        //Looks through waypoints until the last one then moving back to the first waypoint.
        if(currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
    }
}
