using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    //Seeking
    public GameObject Ai;
    public GameObject PlayerTarget;

    //Seeking
    [SerializeField]
    private Vector3 velocity;

    public float Mass = 15;
    public float MaxVelocity = 3;
    public float MaxForce = 15;

    //Fleeing
    public bool fleeMode = false;

    void start()
    {
        //Seeking
        velocity = Vector3.zero;
    }

    void Update()
    {
        //Using basic physics to ensure the AI reaches player character.
        //Converting position into velocity by multiplying with Normalized value and multiplying it
        // with Max Velocity.
        Vector3 desiredVelocity = PlayerTarget.transform.position - Ai.transform.position;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity;

        //A steering vector based on starting location and desired velocity.
        //Mass is finetunned to adjust the movement of AI. Larger AI move slower the smaller ones.
        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        //Final step of creating a steering vector.
        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);

  
        //Applying movement to character allowing them to move from their current position.
        //When Flee isn't activated AI will begin to seek target, when it is activated the
        //AI will begin to run away from target.
        if(fleeMode == false)
            Ai.transform.position += velocity * Time.deltaTime;
        else
            Ai.transform.position += (-1 * velocity) * Time.deltaTime;

        //Aids AI to look at direction being moved towards.
        //Makes AI move towards the target.
        Ai.transform.position = new Vector3(Ai.transform.position.x, 1, Ai.transform.position.z);
        Ai.transform.forward = velocity.normalized;
    }
}