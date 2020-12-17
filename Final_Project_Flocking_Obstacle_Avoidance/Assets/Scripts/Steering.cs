using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

public abstract class Steering : ScriptableObject
{
    //Gets overridden by the derived classes
    public abstract Vector2 GetSteering(Boid boid, List<Transform> context, Flock flock);
}