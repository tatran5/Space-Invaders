using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static float timeEffective = 7f;
    public float speed = 0.8f;

    public float shipSpeedUpFactor = 1.5f;
    public static float invaderSlowDownFactor = 0.5f;
    public static float invaderBulletSlowDownFactor = 0.5f;
    public ResourceType type;

    public AudioClip attainedSounce;

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
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ship"))
        {
            gameObject.active = false;
            Destroy(this);
            AudioSource.PlayClipAtPoint(attainedSounce, transform.position);

            if (type == ResourceType.InvaderSlowDown)
            {
                Invader[] invaders = GameObject.FindObjectsOfType<Invader>();
                for (int i = 0; i < invaders.Length; i++)
                {
                    invaders[i].TriggerSlowDown(invaderSlowDownFactor, timeEffective);
                }
            }
            else if (type == ResourceType.ShipFaster)
            {
                collider.gameObject.GetComponent<Ship>().ShipFaster(timeEffective, shipSpeedUpFactor);
            }
            else
            {
                InvaderBullet[] invaderBullets = GameObject.FindObjectsOfType<InvaderBullet>();
                for (int i = 0; i < invaderBullets.Length; i++)
                {
                    invaderBullets[i].TriggerSlowDown(invaderBulletSlowDownFactor, timeEffective);
                }
            }
        }
    }
}


// for 10 seconds
public enum ResourceType
{
    InvaderSlowDown = 0,
    ShipFaster = 1,
    InvaderBulletSlowDown = 2
}