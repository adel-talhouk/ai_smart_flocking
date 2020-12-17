using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[CreateAssetMenu(menuName = "Flock/Behaviour/CohesionSteering")]
public class CohesionSteering : Steering
{
    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        //If there are no neighbours, return zero vector2
        if (context.Count == 0)
            return Vector2.zero;

        //Get the average of all neighbour positions
        Vector2 averagePos = Vector2.zero;

        foreach (Transform item in context)
        {
            averagePos += (Vector2)item.position;
        }

        averagePos /= context.Count;

        //Create the difference vector
        averagePos -= (Vector2)boid.transform.position;

        return averagePos;
    }
}