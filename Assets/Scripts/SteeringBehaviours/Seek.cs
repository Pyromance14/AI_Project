using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject Ai;
    public GameObject PlayerTarget;

    [SerializeField]
    private Vector3 velocity;

    public float Mass = 15;
    public float MaxVelocity = 3;
    public float MaxForce = 15;

    public bool fleeMode = false;

    void start()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        Vector3 desiredVelocity = PlayerTarget.transform.position - Ai.transform.position;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity;

        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);

        if(fleeMode == false)
            Ai.transform.position += velocity * Time.deltaTime;
        else
            Ai.transform.position += (-1 * velocity) * Time.deltaTime;


        Ai.transform.position = new Vector3(Ai.transform.position.x, 1, Ai.transform.position.z);
        Ai.transform.forward = velocity.normalized;
    }
}