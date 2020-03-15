using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum ObjectType
{
    InvaderObj,
    ShipObj
}

public class Global : MonoBehaviour
{
    public float horizontalBound = 8f;
    public bool hasWon = false;
    public Vector3 gravity;

    // Player / ship
    public int score = 0;
    public int lives = 3;

    // Invaders
    public GameObject invaderPrefab;
    public int defaultScoreInvader = 200;
    public float firstRowZ;
    public int numRowFromShip = 10;
    public int invadersPerRow = 20;
    public int invadersRows = 3;
    public float distanceBetweenInvaders;
    public int invadersRemaining;
    public int invadersLeftTriggerBoss = 10;

    // Boss
    public GameObject bossPrefab;

    // UI
    public GameObject scoreTextObj;
    public GameObject livesTextObj;
    public GameObject invadersLeftObj;
    public GameObject switchCamButtonObj;

    // Camera
    public GameObject overheadCamera;
    public GameObject shipCamera;
    public bool shakeCamera = false;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.4f;
    public float shakeTimeElapsed = 0f;
    Vector3 shakeOriginalPos;

    // Explosion
    public int numParticlesInvader = 50;
    public int numParticlesShip = 150;
    public int particleMaxSpeed = 20;
    public int particleMinSpeed = 10;
    public GameObject particlePrefab;

    // Resource carrier
    public float intervalSpawnResourceCarrier = 5f;
    public GameObject resourceCarrierPrefab;
    float spawnResourceCarrierTimeElapsed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        DebrisParticle.shipY = GameObject.Find("ShipPrefab").transform.position.y;
        DontDestroyOnLoad(this.gameObject);
        SetupCamera();
        UpdateUI();
        switchCamButtonObj.GetComponent<Button>().onClick.AddListener(SwitchCamera);
        SetupInvaders();
    }

    // Update is called once per frame
    void Update()
    {
        HandleShake();
        SpawnResourceCarrier();
    }

    void SpawnResourceCarrier()
    {
        if (spawnResourceCarrierTimeElapsed > intervalSpawnResourceCarrier)
        {
            Vector3 pos = new Vector3(-horizontalBound - 5f, 0, firstRowZ);
            Instantiate(resourceCarrierPrefab, pos, Quaternion.identity);
            spawnResourceCarrierTimeElapsed = 0f;
        } else
        {
            spawnResourceCarrierTimeElapsed += Time.deltaTime;
        }
    }

    public void GenerateExplodeParticles(Vector3 pos, ObjectType objType)   
    {
        int numPar;
        if (objType == ObjectType.InvaderObj)
        {
            numPar = numParticlesInvader;
        } else
        {
            numPar = numParticlesShip;
        }
        
        for (int i = 0; i < numPar; i++)
        {
            Instantiate(particlePrefab, pos, Random.rotation);
        }
    }

    public void InvaderShot(Collider collider)
    {
        score += (collider.gameObject.GetComponent<Invader>().numMoveDown + 1) * defaultScoreInvader;
        Invader invader = collider.gameObject.GetComponent<Invader>();
        GenerateExplodeParticles(invader.transform.position, ObjectType.InvaderObj);
        invader.Die();
        invadersRemaining--;
        UpdateUI();
        if (invadersRemaining == 0)
        {
            hasWon = true;
            SceneManager.LoadScene(sceneName: "GameOverScene");
        } else if (invadersRemaining == invadersLeftTriggerBoss)
        {
            Invader[] invaders = FindObjectsOfType<Invader>();
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].Pause();
            }
            TriggerBoss();
        }
    }


    void TriggerBoss()
    {
        Vector3 pos = new Vector3(0, 0, firstRowZ);
        Instantiate(bossPrefab, pos, Quaternion.identity);
    }


    public void HandleShake()
    {
        if (shakeCamera)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            shipCamera.transform.localPosition = new Vector3(x, y, shakeOriginalPos.z);
            shakeTimeElapsed += Time.deltaTime;
            if (shakeTimeElapsed > shakeDuration)
            {
                shipCamera.transform.localPosition = shakeOriginalPos;
                Debug.Log("shakeOriginalPos2 " + shakeOriginalPos);
                shakeCamera = false;
                shakeTimeElapsed = 0;
            }
        }
    }

    public void InvaderReachShip()
    {
        SceneManager.LoadScene(sceneName: "GameOverScene");
    }

    public void ShipShot()
    {
        if (lives > 0)
        {
            GenerateExplodeParticles(GameObject.Find("ShipPrefab").GetComponent<Ship>().transform.position, ObjectType.ShipObj);
            lives--;
            shakeCamera = true;
            shakeOriginalPos = shipCamera.transform.localPosition;
            Debug.Log("shakeOriginalPos " + shakeOriginalPos);
            UpdateUI();
        } else
        {
            SceneManager.LoadScene(sceneName: "GameOverScene");
        }
    }

    void SetupInvaders()
    {
        invadersRemaining = invadersRows * invadersPerRow;
        distanceBetweenInvaders = (float)(horizontalBound * 2) / (float)(invadersPerRow);
        firstRowZ = numRowFromShip * distanceBetweenInvaders + GameObject.Find("ShipPrefab").GetComponent<Ship>().transform.position.z;
        for (int r = 0; r < invadersRows; r++)
        {
            for (int i = 0; i < invadersPerRow; i++)
            {
                float invaderPosX = -horizontalBound + i * distanceBetweenInvaders;
                float invaderPosZ = firstRowZ + r * distanceBetweenInvaders;
                Instantiate(invaderPrefab, new Vector3(invaderPosX, 0, invaderPosZ), Quaternion.identity);
            }
        }
    }

    void UpdateUI()
    {
        scoreTextObj.GetComponent<Text>().text = "Score: " + score.ToString();
        livesTextObj.GetComponent<Text>().text = "Lives: " + lives.ToString();
        invadersLeftObj.GetComponent<Text>().text = "Invaders: " + invadersRemaining.ToString();
    }

    void SetupCamera()
    {
        overheadCamera.GetComponent<Camera>().enabled = true;
        overheadCamera.GetComponent<Camera>().orthographic = true;
        shipCamera.GetComponent<Camera>().enabled = false;
    }

    void SwitchCamera()
    {
        GameObject backgroundObj = GameObject.Find("Background");
        float currentZAngle = backgroundObj.transform.eulerAngles.z;
        float currentYAngle = backgroundObj.transform.eulerAngles.y;
        float currentXAngle = backgroundObj.transform.eulerAngles.x;
        Debug.Log("currentX: " + currentXAngle);
        Debug.Log("currentY: " + currentYAngle);
        Debug.Log("currentZ: " + currentZAngle);
        if (currentXAngle > 0)
        {
            backgroundObj.transform.Rotate(-90, 0, 0, Space.World);
            backgroundObj.transform.Translate(0, 0, 15, Space.World);
            backgroundObj.transform.localScale *= 4;
        }
        else
        {
            backgroundObj.transform.Translate(0, 0, -15, Space.World);
            backgroundObj.transform.Rotate(90, 0, 0, Space.World);
            backgroundObj.transform.localScale /= 4;
        }
        overheadCamera.GetComponent<Camera>().enabled = !overheadCamera.GetComponent<Camera>().enabled;
        shipCamera.GetComponent<Camera>().enabled = !shipCamera.GetComponent<Camera>().enabled;
    }

}
