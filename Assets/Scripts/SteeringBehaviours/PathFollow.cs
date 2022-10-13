using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Waypoints waypoints;
    public float moveSpeed = 5f;
    private float distanceThreshold = 0.1f;

    //The waypoint target being moved towards by the AI
    private Transform currentWaypoint;

    void Start()
    {
        //Sets starting position at first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //Sets next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    void Update()
    {
        //Makes the AI move from one position to the other without warping there.
        //When AI is close to the target waypoint it'll begin to move to the next one.
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
    }
}
