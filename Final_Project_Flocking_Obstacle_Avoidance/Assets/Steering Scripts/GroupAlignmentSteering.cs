using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[CreateAssetMenu(menuName = "Flock/Behaviour/GroupAlignmentSteering")]
public class GroupAlignmentSteering : Steering
{
    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        //If there are no neighbours, keep heading in the same direction
        if (context.Count == 0)
            return boid.transform.up;

        //Get the average of all neighbour directions
        Vector2 averageHeading = Vector2.zero;

        foreach (Transform item in context)
        {
            averageHeading += (Vector2)item.transform.up;
        }

        averageHeading /= context.Count;

        return averageHeading;
    }
}
