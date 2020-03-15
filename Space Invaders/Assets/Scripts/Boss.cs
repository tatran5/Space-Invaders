using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public int defaultHp = 50;
    public float timePerStage = 20f;
    public float acceleration = 1.2f;
    float horBound;
    float highestAltitude;

    int numStage = 2;
    int currentStage = 0;

    float timeElapsedStage = 0;

    public float timeBetweenAttack = 1f;
    float timeElapsedSinceLastAttack = 0;

    public float timeMovementMin = 2f; 
    public float timeMovementMax = 5f;
    public float timeMovement;
    public float timeElapsedMovement = 0;
    public float velMaxX = 15f;
    public float vel;

    // Stage 0
    public float distVer = 1.1f;
    public float distHor = 1.1f;
    public GameObject bigBulletPrefab;
    bool lastLowerRight = false;

    // Start is called before the first frame update
    void Start()
    {
        hp = defaultHp;
        Global global = GameObject.Find("Global").GetComponent<Global>();
        horBound = 0.5f * GameObject.Find("Platform").transform.localScale.x;
        highestAltitude = 2f * global.firstRowZ + 3;
        timeMovement = Random.Range(timeMovementMin, timeMovementMax);
        vel = Random.Range(-velMaxX, velMaxX);
    }

    void HandleMovement()
    {
        float horBound = 0.5f * GameObject.Find("Platform").transform.localScale.x;
        if (transform.position.x < -horBound)
        {
            transform.position = new Vector3(-horBound, 0, transform.position.z);
            vel = Random.Range(0, velMaxX);
            timeMovement = Random.Range(timeMovementMin, timeMovementMax);
            timeElapsedMovement = 0;
        } else if (transform.position.x > horBound)
        {
            transform.position = new Vector3(horBound, 0, transform.position.z);
            vel = Random.Range(-velMaxX, 0);
            timeMovement = Random.Range(timeMovementMin, timeMovementMax);
            timeElapsedMovement = 0;
        } else if (timeElapsedMovement > timeMovement)
        {
            vel = Random.Range(-velMaxX, velMaxX);
            timeMovement = Random.Range(timeMovementMin, timeMovementMax);
            timeElapsedMovement = 0;
        }
        transform.position += Time.deltaTime * new Vector3(vel, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //if (currentStage % numStage == 0)
        //{
        if (timeElapsedSinceLastAttack > timeBetweenAttack)
        {
            timeElapsedSinceLastAttack = 0;
            AttackStage0();
        }
        timeElapsedStage += Time.deltaTime;
        timeElapsedSinceLastAttack += Time.deltaTime;
   //     CheckForNewStage();
        //} else
        //{

        //}
        //timeElapsedStage += Time.deltaTime;
    }

    void CheckForNewStage()
    {
        if (timeElapsedStage > timePerStage)
        {
            timeElapsedStage = 0;
            timeElapsedSinceLastAttack = 0;
            currentStage = (currentStage + 1) % numStage;
        }
    }

    void AttackStage0()
    {
        float offset = Random.Range(-2f, 2f);
        Vector3 bulletScale = bigBulletPrefab.gameObject.transform.localScale;
        float bulletWidth = bulletScale.x;
        float minX = -horBound + offset;
        float maxX = horBound + offset;
        float shipWidth = GameObject.Find("ShipPrefab").transform.localScale.x;
        int numBullets = (int) (2f * Mathf.Abs(minX) / (bulletWidth + shipWidth));
        if (!lastLowerRight) { 
            for (int i = 0; i < numBullets; i++)
            {
                float posX = minX + i * (bulletWidth + distHor);
                float posZ = highestAltitude - i * distVer; 
                Vector3 pos = new Vector3(posX, 0, posZ);
                Instantiate(bigBulletPrefab, pos, Quaternion.identity);
            }
        } else
        {
            for (int i = 0; i < numBullets; i++)
            {
                float posX = maxX - i * (bulletWidth + distHor);
                float posZ = highestAltitude - i * distVer;
                Vector3 pos = new Vector3(posX, 0, posZ);
                Instantiate(bigBulletPrefab, pos, Quaternion.identity);
            }
        }
        lastLowerRight = !lastLowerRight;
    }

    void AttackStage1()
    {
        Vector3 shipPos = GameObject.Find("ShipPrefab").GetComponent<Ship>().transform.position;
    }

    public void GotHit()
    {
        if (hp > 0)
        {
            hp--;
            Debug.Log("Boss: hp = " + hp);
        }
        else
        {
            Global global = GameObject.Find("Global").GetComponent<Global>();
            global.GenerateExplodeParticles(transform.position, ObjectType.InvaderObj);

            // Make all invaders continue
            Invader[] invaders = GameObject.FindObjectsOfType<Invader>();
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].Unpause();
            }
            gameObject.active = false;
            Destroy(this);
        }
    }
}