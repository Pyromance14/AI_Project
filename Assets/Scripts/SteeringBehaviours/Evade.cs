using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : MonoBehaviour
{
    /*
    public float max_velocity = 5;
    public Vector3 max_force = (20, 0, 20);
    public float T;
    public float mass = 3;

    Vector3 velocity = Vector3.zero;
    public Transform target;

    void Update()
    {
        Vector3 desired_velocity = this.transform.position - target.transform.position;
        T = desired_velocity.magnitude;
        T /= max_velocity;
        desired_velocity = Normalize();
        desired_velocity *= max_velocity;
        Vector3 steering = new Vector3(0, 0, 0);
        steering = desired_velocity - velocity;

        if(steering.magnitude > max_force.magnitude)
        {
            steering.Normalize();
            steering *= max_force.magnitude;
        }

        steering = steering / mass;

        if ((velocity.magnitude + steering.magnitude) > max_velocity)
        {
            velocity.Normalize(); 
            velocity *= max_velocity;
        }

        velocity += steering * Time.deltaTime;
        transform.position += (Vector3)velocity * T *  Time.deltaTime;
        
    }

    */

}
