using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[CreateAssetMenu(menuName = "Flock/Behaviour/CompositeSteering")]
public class CompositeSteering : Steering
{
    //The steering behaviours we want to combine and the weights
    public Steering[] flockSteeringBehaviours;
    public float[] correspondingWeights;


    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        //Make sure the arrays are of the same length
        if (correspondingWeights.Length != flockSteeringBehaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        //Calculate the combines behaviours
        Vector2 combinedMovement = Vector2.zero;

        //Iterate through the behaviours
        for (int i = 0; i < flockSteeringBehaviours.Length; i++)
        {
            Vector2 partialMove = flockSteeringBehaviours[i].GetSteering(boid, context, flock) * correspondingWeights[i];

            //Make sure the weight is respected
            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > correspondingWeights[i] * correspondingWeights[i])
                {
                    partialMove.Normalize();
                    partialMove *= correspondingWeights[i];
                }

                //Add it to the combined movement vector
                combinedMovement += partialMove;
            }
        }

        return combinedMovement;
    }
}
