using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBullet : MonoBehaviour
{
    public float speed = 1f;
    public AudioClip invaderExplodeSound;
    public AudioClip wallDamageSound;
    public float invaderExplodeSoundVolume = 0.5f;

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
        if (transform.position.z > global.firstRowZ + 5 || transform.position.z < shipObj.transform.position.z - 5)
        {
            gameObject.active = false;
            Destroy(this);
        }
        else
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Invader") && !collider.gameObject.GetComponent<Invader>().isShot && !collider.gameObject.GetComponent<Invader>().isPaused)
        {
            gameObject.active = false;
            Destroy(this);
            AudioSource.PlayClipAtPoint(invaderExplodeSound, transform.position, invaderExplodeSoundVolume);
            Global global = GameObject.Find("Global").GetComponent<Global>();
            global.InvaderShot(collider);
        } else if (collider.gameObject.GetComponent<Boss>() != null)
        {
            gameObject.active = false;
            Destroy(this);
            Debug.Log("ShipBullet: hit boss");
            collider.gameObject.GetComponent<Boss>().GotHit();
        }
    }   
    
}
