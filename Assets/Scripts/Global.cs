using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // The invaders can only move horizontally within this boundary
    public float lastTimeStep;


    // Start is called before the first frame update
    void Start()
    {
        lastTimeStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lastTimeStep = Time.time;
    }

    public void Lose()
    {

    }
}
