using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //Obstacles in the scene
    [Header("Obstacles in the Scene")]
    public GameObject circleObstacle;
    public GameObject squareObstacle;
    [Range(0.1f, 10.0f)] public float obstacleMoveSpeed = 5.0f;
    public KeyCode switchObstaclesKey = KeyCode.W;

    //Current obstacle
    GameObject currentObstacle;

    // Start is called before the first frame update
    void Start()
    {
        //Start with the on obstacles active
        circleObstacle.SetActive(false);
        squareObstacle.SetActive(false);
        currentObstacle = circleObstacle;
    }

    // Update is called once per frame
    void Update()
    {
        //If we hold left click
        if (Input.GetMouseButton(0))
        {
            MoveObstacle(Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition));
        }

        //If we right click
        if (Input.GetMouseButtonDown(1))
        {
            //Disable the current obstacle
            currentObstacle.SetActive(false);
        }

        //If we press the switch obstacles key
        if (Input.GetKeyDown(switchObstaclesKey))
        {
            //Switch the active obstacles
            SwitchObstacle();
        }
    }

    void SwitchObstacle()
    {
        //If the current obstacle is the circle
        if (currentObstacle == circleObstacle)
        {
            //Activate the square and disable the circle
            squareObstacle.SetActive(true);
            currentObstacle = squareObstacle;
            circleObstacle.SetActive(false);
        }
        //If the current obstacle is the square
        else if (currentObstacle == squareObstacle)
        {
            //Activate the circle and disable the square
            circleObstacle.SetActive(true);
            currentObstacle = circleObstacle;
            squareObstacle.SetActive(false);
        }
    }

    void MoveObstacle(Vector2 position)
    {
        //If an obstacle is active
        if (currentObstacle.activeSelf)
        {
            //Update its position
            currentObstacle.transform.position = position;
        }
        else    //Otherwise
        {
            //Activate the current obstacle
            currentObstacle.SetActive(true);
        }
    }
}
