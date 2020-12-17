using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

public class Flock : MonoBehaviour
{
    //Boids info
    [Header("Boids Information")]
    public Boid boidPrefab;
    public CompositeSteering steering;
    [Range(1.0f, 50.0f)] public float boidSpeed = 5.0f;

    //Boids spawning
    [Header("Boids Spawning Data")]
    [Range(1, 250)] public int numOfInitialBoids = 25;
    [Range(1, 10)] public int boidSpawnMultiplier = 1;

    //Neighbours
    [Header("Neighbourhood Information")]
    [Range(1.0f, 10.0f)] public float neighbourhoodRadius = 1.5f;
    [Range(0.0f, 1.0f)] public float avoidanceRadiusMultiplier = 0.5f;

    //Steering data
    [Header("Steering Data")]
    [Range(0.1f, 1.0f)]
    public float cohesionWeight = 0.5f;
    [Range(0.1f, 1.0f)]
    public float separationWeight = 0.3f;
    [Range(0.1f, 1.0f)]
    public float groupAlignmentWeight = 0.7f;
    [Range(0.1f, 1.0f)]
    public float avoidEdgeWeight = 0.9f;
    [Range(10.0f, 50.0f)]
    public float avoidEdgeRadius = 20.0f;
    [Range(0.1f, 1.0f)]
    public float obstacleAvoidanceWeight = 1.0f;
    [Range(5.0f, 30.0f)]
    public float obstacleDetectionDepth = 10.0f;
    [Range(1.0f, 50.0f)]
    public float obstacleAvoidanceStrength = 10.0f;

    //Original values
    float originalCohesionWeight;
    float originalSeparationWeight;
    float originalGroupAlignmentWeight;
    float originalAvoidEdgeWeight;
    float originalAvoidEdgeRadius;
    float originalObstacleAvoidanceWeight;
    float originalObstacleDetectionDepth;
    float originalObstacleAvoidanceStrength;

    //Utility variables
    float squaredNeighbourhoodRadius;
    float squaredAvoidanceRadius;
    public float SquaredAvoidanceRadius { get { return squaredAvoidanceRadius; } }

    const float BOIDSPAWNDENSITY = 0.08f;
    List<Boid> boidsList = new List<Boid>();

    // Start is called before the first frame update
    void Start()
    {
        squaredNeighbourhoodRadius = neighbourhoodRadius * neighbourhoodRadius;
        squaredAvoidanceRadius = squaredNeighbourhoodRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //Save the original values related to the steering
        GetOriginalValues();
        originalCohesionWeight = cohesionWeight;
        originalSeparationWeight = separationWeight;
        originalGroupAlignmentWeight = groupAlignmentWeight;
        originalAvoidEdgeWeight = avoidEdgeWeight;
        originalAvoidEdgeRadius = avoidEdgeRadius;
        originalObstacleAvoidanceWeight = obstacleAvoidanceWeight;
        originalObstacleDetectionDepth = obstacleDetectionDepth;
        originalObstacleAvoidanceStrength = obstacleAvoidanceStrength;

        //Create the boids
        SpawnBoids(numOfInitialBoids);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player presses the Space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBoids(numOfInitialBoids * boidSpawnMultiplier);
        }

        //Iterate through all the boids
        foreach (Boid boid in boidsList)
        {
            //Context of the boid's neighbourhood
            List<Transform> context = GetNearbyObjects(boid);
            Vector2 move = steering.GetSteering(boid, context, this);
            move *= boidSpeed;

            boid.Move(move);
        }

        //Check if the values changed
        CheckForNewValues();
    }

    List<Transform> GetNearbyObjects(Boid boid)
    {
        List<Transform> context = new List<Transform> ();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(boid.transform.position, neighbourhoodRadius);

        //Go through all the contextColliders
        foreach (Collider2D col in contextColliders)
        {
            //As long as it's not this boid
            if (col != boid.BoidCollider)
            {
                //Add the transform to the context
                context.Add(col.transform);
            }
        }

        return context;
    }

    void SpawnBoids(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            Boid newBoid = Instantiate(
                boidPrefab,
                Random.insideUnitCircle * numOfInitialBoids * BOIDSPAWNDENSITY,     //Spawn inside circle (scales with num of initial boids)
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),      //Random rotation
                transform   //Child of this Flock
                );
            newBoid.name = "Boid " + i;
            boidsList.Add(newBoid);
        }
    }

    void GetOriginalValues()
    {
        for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
        {
            //If we find the cohesion steering
            if (steering.flockSteeringBehaviours[i].GetType() == typeof(SmoothedCohesionSteering))
            {
                //Set the weight
                cohesionWeight = steering.correspondingWeights[i];
            }
            //If we find the separation steering
            else if (steering.flockSteeringBehaviours[i].GetType() == typeof(SeparationSteering))
            {
                //Set the weight
                separationWeight = steering.correspondingWeights[i];
            }
            //If we find the group alignment steering
            else if (steering.flockSteeringBehaviours[i].GetType() == typeof(GroupAlignmentSteering))
            {
                //Set the weight
                groupAlignmentWeight = steering.correspondingWeights[i];
            }
            //If we find the avoid edge steering
            else if (steering.flockSteeringBehaviours[i].GetType() == typeof(AvoidEdgeSteering))
            {
                //Set the weight
                avoidEdgeWeight = steering.correspondingWeights[i];

                //Set the radius
                AvoidEdgeSteering avoidance = (AvoidEdgeSteering)steering.flockSteeringBehaviours[i];
                avoidEdgeRadius = avoidance.radius;
            }
            //If we find the obstacle avoidance steering
            else if (steering.flockSteeringBehaviours[i].GetType() == typeof(ObstacleAvoidanceSteering))
            {
                //Set the weight
                obstacleAvoidanceWeight = steering.correspondingWeights[i];

                //Set the detection depth and the avoidance strength
                ObstacleAvoidanceSteering avoidance = (ObstacleAvoidanceSteering)steering.flockSteeringBehaviours[i];
                obstacleDetectionDepth = avoidance.detectionDepth;
                obstacleAvoidanceStrength = avoidance.avoidanceStrength;
            }
        }
    }

    void CheckForNewValues()
    {
        //Cohesion weight
        if (cohesionWeight != originalCohesionWeight)
        {
            //Find the index of the cohesion
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the cohesion steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(SmoothedCohesionSteering))
                {
                    //Set the weight
                    steering.correspondingWeights[i] = cohesionWeight;
                }
            }
        }

        //Separation weight
        if (separationWeight != originalSeparationWeight)
        {
            //Find the index of the separation
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the separation steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(SeparationSteering))
                {
                    //Set the weight
                    steering.correspondingWeights[i] = separationWeight;
                }
            }
        }

        //Group alignment weight
        if (groupAlignmentWeight != originalGroupAlignmentWeight)
        {
            //Find the index of the group alignment
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the group alignment steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(GroupAlignmentSteering))
                {
                    //Set the weight
                    steering.correspondingWeights[i] = groupAlignmentWeight;
                }
            }
        }

        //Avoid edge weight
        if (avoidEdgeWeight != originalAvoidEdgeWeight)
        {
            //Find the index of the avoid edge
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the avoid edge steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(AvoidEdgeSteering))
                {
                    //Set the weight
                    steering.correspondingWeights[i] = avoidEdgeWeight;
                }
            }
        }

        //Avoid edge radius
        if (avoidEdgeRadius != originalAvoidEdgeRadius)
        {
            //Find the index of the avoid edge
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the avoid edge steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(AvoidEdgeSteering))
                {
                    //Set the radius
                    AvoidEdgeSteering avoidance = (AvoidEdgeSteering)steering.flockSteeringBehaviours[i];
                    avoidance.radius = avoidEdgeRadius;
                }
            }
        }

        //Obstacle avoidance weight
        if (obstacleAvoidanceWeight != originalObstacleAvoidanceWeight)
        {
            //Find the index of the avoid edge
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the avoid edge steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(ObstacleAvoidanceSteering))
                {
                    //Set the weight
                    steering.correspondingWeights[i] = obstacleAvoidanceWeight;
                }
            }
        }

        //Obstacle detection depth
        if (obstacleDetectionDepth != originalObstacleDetectionDepth)
        {
            //Find the index of the avoid edge
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the avoid edge steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(ObstacleAvoidanceSteering))
                {
                    //Set the depth
                    ObstacleAvoidanceSteering avoidance = (ObstacleAvoidanceSteering)steering.flockSteeringBehaviours[i];
                    avoidance.detectionDepth = obstacleDetectionDepth;
                }
            }
        }

        //Obstacle detection depth
        if (obstacleAvoidanceStrength != originalObstacleAvoidanceStrength)
        {
            //Find the index of the avoid edge
            for (int i = 0; i < steering.flockSteeringBehaviours.Length; i++)
            {
                //If we find the avoid edge steering
                if (steering.flockSteeringBehaviours[i].GetType() == typeof(ObstacleAvoidanceSteering))
                {
                    //Set the depth
                    ObstacleAvoidanceSteering avoidance = (ObstacleAvoidanceSteering)steering.flockSteeringBehaviours[i];
                    avoidance.avoidanceStrength = obstacleAvoidanceStrength;
                }
            }
        }
    }
}
