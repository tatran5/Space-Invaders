using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    // Start is called before the first frame update
    public float initSpeedMax = 10;
    public float initSpeedMin = 3;
    public float timeAlive = 1f;
    Rigidbody rb;
    float timeElapsed = 0;


    void Start()
    {
        Physics.gravity = GameObject.Find("Global").GetComponent<Global>().gravity;
        float initSpeed = Random.Range(initSpeedMin, initSpeedMax);
        GetComponent<Rigidbody>().velocity = initSpeed * new Vector3(Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed > timeAlive)
        {
            gameObject.active = false;
            Destroy(this);
        }
        timeElapsed += Time.deltaTime;
    }
}
