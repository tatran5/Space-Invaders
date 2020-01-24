using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    Vector3 velocity = new Vector3(3, 0, 0);
    float speedIncrement = 2;
    float moveDownIncrement = 1;
    float currentMoveDownIncrement = 0;
    float horBound = 15; // the invader can only move horizontally within this bound 
    bool moveLeft = false;
    bool moveDown = false;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        
        Global global = GameObject.FindWithTag("Global").GetComponent<Global>();
        float deltaTime = Time.deltaTime;

        HandleReachBoundary();
        HandleMoveDown(deltaTime);
        transform.position += deltaTime * velocity;

        // Reach the horizontal level of where the ship is -> lose condition for the player
        Ship ship = GameObject.FindWithTag("Ship").GetComponent<Ship>();
        if (transform.position.z < ship.transform.position.x)
        {
            velocity = new Vector3(0, 0, 0);
            global.Lose();
        }
    }

    void HandleMoveDown(float deltaTime)
    {
        // Need to stop move down once hits 
        if (moveDown && currentMoveDownIncrement > moveDownIncrement)
        {
            moveDown = false;

            if (moveLeft)
            {
                velocity.x = -Mathf.Abs(velocity.z) - speedIncrement;
            } else
            {
                velocity.x = Mathf.Abs(velocity.z) + speedIncrement;
            }
            velocity.z = 0;
            currentMoveDownIncrement = 0;
        } else if (moveDown && currentMoveDownIncrement < moveDownIncrement)
        {
            currentMoveDownIncrement += deltaTime * Mathf.Abs(velocity.z);
        }
    }

    void HandleReachBoundary()
    {
        // check if reach left or right boundary,
        // move the invader closer to the ship and 
        // reverse the horizontal moving direction
        if (transform.position.x < -horBound || transform.position.x > horBound)
        {
            if (moveLeft)
            {
                transform.position = new Vector3(-horBound, transform.position.y, transform.position.z);
            } else
            {
                transform.position = new Vector3(horBound, transform.position.y, transform.position.z);
            }

            moveLeft = !moveLeft;
            moveDown = true;
            velocity.z = -Mathf.Abs(velocity.x);
            velocity.x = 0;
        }
    }

    void Die()
    {

    }
}
