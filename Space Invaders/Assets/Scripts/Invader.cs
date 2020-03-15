using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public int numMoveDown = 0;
    public Vector3 velocity = new Vector3(3, 0, 0);
    public float speedIncrement = 1.7f;
    
    // Related to resources obtained by the ship
    float slowDownFactor;
    float timeSlowDown;
    float timeElapsedSlowDown;
    bool slowDown;

    public bool isShot = false;
    public bool isPaused = false;
    bool moveLeft = false;
    bool moveRight = false;
    bool moveDown = false;
    float currentMoveDownIncrement = 0;

    public GameObject bulletPrefab;
    public float bulletSpeed = -0.12f;
    public float timeFactorNumerator = 1f;
    public float timeFactorDenominator = 1.0006f;
    public float timeFactorMoveDown = 1.002f;

    public GameObject debrisParticles;
    public int numDebrisParticles = 5;

    public void TriggerSlowDown(float slowDownFactor, float timeSlowDown)
    {
        slowDown = true;
        this.timeSlowDown = timeSlowDown;
        this.timeElapsedSlowDown = 0;
        this.slowDownFactor = slowDownFactor;
        velocity = slowDownFactor * velocity;
    }

    void HandleSlowDown()
    {
        if (slowDown && timeElapsedSlowDown > timeSlowDown)
        {
            slowDown = false;
            timeElapsedSlowDown = 0;
            velocity /= slowDownFactor;
        } else if (slowDown)
        {
            timeElapsedSlowDown += Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShot && !isPaused)
        {
            float deltaTime = Time.deltaTime;
            HandleShoot();
            HandleReachBoundary();
            HandleReachShip();
            HandleMoveDown(deltaTime);
            transform.position += deltaTime * velocity;
        }
        HandleSlowDown();
    }

    public void HandleReachShip()
    {
        Ship ship = GameObject.Find("ShipPrefab").GetComponent<Ship>();
        if (!isShot && transform.position.z <= ship.transform.position.z)
        {
            GameObject[] invadersObjs = GameObject.FindGameObjectsWithTag("Invader");
            for (int i = 0; i < invadersObjs.Length; i++)
            {
                invadersObjs[i].GetComponent<Invader>().velocity = new Vector3(0, 0, 0);
            }
            GameObject.Find("Global").GetComponent<Global>().InvaderReachShip();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameObject.Find("Platform"))
        {
            GenerateDebris();
        }
    }

    void GenerateDebris()
    {
        for (int i = 0; i < numDebrisParticles; i++)
        {
            Quaternion quat = Random.rotation;
            quat.eulerAngles.Set(0, 0, quat.eulerAngles.z);
            Instantiate(debrisParticles, transform.position, quat);
        }
    }

    public void HandleMoveDown(float deltaTime)
    {
        Global global = GameObject.Find("Global").GetComponent<Global>();
        // Need to stop move down once hits 
        if (moveDown && currentMoveDownIncrement > global.distanceBetweenInvaders)
        {
            moveDown = false;

            if (moveLeft)
            {
                velocity.x = -Mathf.Abs(velocity.z) - speedIncrement;
            }
            else
            {
                velocity.x = Mathf.Abs(velocity.z) + speedIncrement;
            }
            transform.position += new Vector3(0, 0, currentMoveDownIncrement - global.distanceBetweenInvaders); // ensure that all invaders in one row line up
            velocity.z = 0;
            currentMoveDownIncrement = 0;
            numMoveDown++;
        }
        else if (moveDown && currentMoveDownIncrement < global.distanceBetweenInvaders)
        {
            currentMoveDownIncrement += deltaTime * Mathf.Abs(velocity.z);
        }
    }

    public void HandleReachBoundary()
    {
        Global global = GameObject.Find("Global").GetComponent<Global>();
        if (transform.position.x < -global.horizontalBound || transform.position.x > global.horizontalBound)
        {
            if (moveLeft)
            {
                transform.position = new Vector3(-global.horizontalBound, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(global.horizontalBound, transform.position.y, transform.position.z);
            }

            moveLeft = !moveLeft;
            moveDown = true;
            velocity.z = -Mathf.Abs(velocity.x);
            velocity.x = 0;
        }
    }

    public void HandleShoot()
    {
        float denominator = 1;
        for (int i = 0; i < numMoveDown; i++)
        {
            denominator *= timeFactorDenominator;
        }

        float randNum = Random.Range(0.0f, 0.9999f);
        float threshHold = timeFactorNumerator / denominator;
        if ((randNum * timeFactorMoveDown > threshHold && moveDown) || randNum > threshHold)
        {
            // Make sure that the bullet is spawned at the tip of the ship
            Vector3 bulletPos = transform.position;
            bulletPos.z -= 1.5f * transform.localScale.z;

            // Instantiate the bullet
            InvaderBullet bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.identity).GetComponent<InvaderBullet>();
        }
    }

    public void Die()
    {
        isShot = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        Physics.gravity = GameObject.Find("Global").GetComponent<Global>().gravity;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Unpause()
    {
        isPaused = false;
    }
}
