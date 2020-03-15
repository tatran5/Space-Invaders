using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float speed;
    public GameObject bulletPrefab;
    public AudioClip shootSound;

    bool speedUp = false;
    float timeSpeedUp = 0;
    float timeElapsedSpeedUp = 0;
    float speedUpFactor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleShoot();
        HandleMoveLeft();
        HandleMoveRight();
        AdjustSpeed();
    }

    void HandleShoot()
    {
        if (Input.GetButtonDown("Up"))
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
             // Make sure that the bullet is spawned at the tip of the ship
            Vector3 bulletPos = transform.position;
            bulletPos.z += 1.5f * transform.localScale.z;
            Instantiate(bulletPrefab, bulletPos, Quaternion.identity).GetComponent<ShipBullet>();
        }
    }

    void HandleMoveLeft()
    {
        if (Input.GetButton("Left"))
        {
            float horBound = GameObject.Find("Platform").transform.localScale.x / 2f;
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -horBound)
            {
                transform.position = new Vector3(-horBound, transform.position.y, transform.position.z);
            }
        }
    }

    void HandleMoveRight()
    {
        if (Input.GetButton("Right")) 
        {
            float horBound = GameObject.Find("Platform").transform.localScale.x / 2f;
            Debug.Log("horBound" + horBound);
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x > horBound)
            {
                transform.position = new Vector3(horBound, transform.position.y, transform.position.z);
            }
        }
    }

    // Adjust speed back to normal in case the resource attained is no longer effective
    void AdjustSpeed()
    {
        if (speedUp && timeElapsedSpeedUp > timeSpeedUp)
        {
            speed /= speedUpFactor;
            speedUp = false;
            timeElapsedSpeedUp = 0;
        } else if (speedUp)
        {
            timeElapsedSpeedUp += Time.deltaTime;
        }
    }

    public void ShipFaster(float time, float speedUpFactor)
    {
        speedUp = false;
        timeSpeedUp = time;
        this.speedUpFactor = speedUpFactor;
        speed = speedUpFactor * speed;
    }
}
