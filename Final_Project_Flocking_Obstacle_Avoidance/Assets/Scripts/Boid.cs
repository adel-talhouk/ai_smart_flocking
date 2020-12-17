using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

[RequireComponent(typeof(Collider2D))]
public class Boid : MonoBehaviour
{
    Collider2D boidCollider;
    public Collider2D BoidCollider { get { return boidCollider; } }

    void Start()
    {
        boidCollider = GetComponent<Collider2D>();

        //Make it a random colour
        Color randomColour = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = randomColour;
    }

    public void Move(Vector2 velocity)
    {
        //Head towards the velocity vector
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
