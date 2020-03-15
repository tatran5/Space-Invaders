using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderBullet : MonoBehaviour
{
    public float speed = 0.4f;
    public AudioClip wallDamageSound;

    float slowDownFactor;
    float timeSlowDown;
    float timeElapsedSlowDown;
    bool slowDown;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Global global = GameObject.Find("Global").GetComponent<Global>();
        GameObject shipObj = GameObject.Find("ShipPrefab");

        // If the bullet is too far away, destroy it to free up space
        if (transform.position.z < shipObj.transform.position.z - 5)
        {
            gameObject.active = false;
            Destroy(this);
        }
        else
        {
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            HandleSlowDown();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ship"))
        {
            gameObject.active = false;
            Destroy(this);
            GameObject.Find("Global").GetComponent<Global>().ShipShot();
        }
        else if (collider.CompareTag("Wall"))
        {
            gameObject.active = false;
            Destroy(this);
            Wall wall = collider.gameObject.GetComponent<Wall>();
            wall.Shot();
            //AudioSource.PlayClipAtPoint(wallDamageSound, transform.position);
        }
    }

   public void TriggerSlowDown(float slowDownFactor, float timeSlowDown)
    {
        slowDown = true;
        this.timeSlowDown = timeSlowDown;
        this.timeElapsedSlowDown = 0;
        this.slowDownFactor = slowDownFactor;
        speed = slowDownFactor * speed;
    }

    void HandleSlowDown()
    {
        if (slowDown && timeElapsedSlowDown > timeSlowDown)
        {
            slowDown = false;
            timeElapsedSlowDown = 0;
            speed /= slowDownFactor;
        }
        else if (slowDown)
        {
            timeElapsedSlowDown += Time.deltaTime;
        }
    }
}
