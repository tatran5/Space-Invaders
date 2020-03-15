using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisParticle : MonoBehaviour
{
    public static float shipY;
    public static float timeAlive = 7f;
    public float timeElapse = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y != shipY)
        {
            transform.position = new Vector3(transform.position.x, shipY, transform.position.z);
        }
        if (timeElapse > timeAlive)
        {
            gameObject.active = false;
            Destroy(this);
        }
        timeElapse += Time.deltaTime;
    }
}
