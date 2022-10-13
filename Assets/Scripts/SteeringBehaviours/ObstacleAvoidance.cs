using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleAvoidance : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent nav;

    void Start()
    {
        //Assigns player
        player = GameObject.FindGameObjectWithTag("Player").transform;

        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Makes the player the primary Target of the AI.
        nav.SetDestination(player.position);
    }
}
