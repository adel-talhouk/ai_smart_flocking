using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[CreateAssetMenu(menuName = "Flock/Behaviour/AvoidEdgeSteering")]
public class AvoidEdgeSteering : Steering
{
    public Vector2 center;
    public float radius = 10.0f;

    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        //How far away the boid is from the center
        Vector2 centerOffset = center - (Vector2)boid.transform.position;
        float portionAway = centerOffset.magnitude / radius;

        //If the boid is very close to the edge of the circle
        if (portionAway >= 0.9f)
        {
            return centerOffset * portionAway * portionAway;
        }

        return Vector2.zero;
    }
}