using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // General
    public float horBound = 15; // horizontal boundary for both ship and invaders
    public GameObject prefab; // mainly used to create invaders initially

    // Related to the ship
    public int score = 0;
    int livesRemaining = 3;

    // Related to the invaders
    int defaultScoreInvader = 100;
    float firstRowZ = 18; //position of the row furthest from the player in the z direction
    float distBwRows = 1;
    int invadersPerRow = 20;
    int invadersRows = 5;
    int invadersRemaining;

    // Start is called before the first frame update
    void Start()
    {
        float distBwInvaders = (float) (horBound * 2) / (float) (invadersPerRow); // horizontally
        for (int r = 0; r < invadersRows; r++)
        {
            for (int i = 0; i < invadersPerRow; i++)
            {
                float invaderPosX = -horBound + i * distBwInvaders;
                float invaderPosZ = firstRowZ + r * distBwRows;
                Instantiate(prefab, new Vector3(invaderPosX, 0, invaderPosZ), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Lose()
    {

    }

    public void AddScore(int numMoveDown)
    {
        score += numMoveDown * defaultScoreInvader;
    }
}
