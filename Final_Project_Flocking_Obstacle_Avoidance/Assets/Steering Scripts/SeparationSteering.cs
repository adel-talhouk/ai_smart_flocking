using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[CreateAssetMenu(menuName = "Flock/Behaviour/SeparationSteering")]
public class SeparationSteering : Steering
{
    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        //If there are no neighbours, return zero vector2
        if (context.Count == 0)
            return Vector2.zero;

        //Move to avoid
        Vector2 avoidanceMove = Vector2.zero;
        int numOfBoidsToAvoid = 0;

        foreach (Transform item in context)
        {
            //Make sure the nighbour is in the avoidance radius
            if (Vector2.SqrMagnitude(item.position - boid.transform.position) < flock.SquaredAvoidanceRadius)
            {
                //Move away from the neighbour
                numOfBoidsToAvoid++;
                avoidanceMove += (Vector2)(boid.transform.position - item.position);
            }
        }

        if (numOfBoidsToAvoid > 0)
            avoidanceMove /= numOfBoidsToAvoid;

        return avoidanceMove;
    }
}
