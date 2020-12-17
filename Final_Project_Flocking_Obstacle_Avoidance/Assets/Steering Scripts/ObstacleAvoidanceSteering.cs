using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/ObstacleAvoidanceSteering")]
public class ObstacleAvoidanceSteering : Steering
{
    [Range(5.0f, 30.0f)] public float detectionDepth = 10.0f;
    [Range(1.0f, 50.0f)] public float avoidanceStrength = 10.0f;

    public LayerMask obstacleLayer;

    public override Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock)
    {
        Vector2 avoidanceVector = Vector2.zero;

        //Effeciency improvement: check if there is an obstacle in the context
        foreach (Transform item in context)
        {
            if (item.CompareTag("Obstacle"))
            {
                //Cast a ray
                RaycastHit2D objectHit = Physics2D.Raycast(boid.transform.position, boid.transform.up, 
                    detectionDepth, obstacleLayer, 0, 0);

                //If it hits an obstacle
                if (objectHit && objectHit.collider.CompareTag("Obstacle"))
                {
                    //If the normal is pointing to the left
                    if (objectHit.normal.x < 0)
                    {
                        //Steer left
                        avoidanceVector = new Vector2(-flock.boidSpeed * avoidanceStrength, flock.boidSpeed * avoidanceStrength);
                    }
                    else    //Otherwise
                    {
                        //Steer right
                        avoidanceVector = new Vector2(flock.boidSpeed * avoidanceStrength, flock.boidSpeed * avoidanceStrength);
                    }
                }
            }
        }

        return avoidanceVector;
    }
}
