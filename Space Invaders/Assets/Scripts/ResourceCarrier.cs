using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCarrier : MonoBehaviour
{
    public float speed = 0.8f;
    public float epsilon = 0.05f;
    bool dropped = false;
    float dropLocationX;

    public GameObject rInvaderSlowDownPrefab;
    public GameObject rShipFasterPrefab;
    public GameObject rInvaderBulletSlowDownPrefab;
    public GameObject resourcePrefab;

    // Start is called before the first frame update
    void Start()
    {
        float maxRange = 0.5f * GameObject.Find("Platform").transform.localScale.x;
        dropLocationX = Random.Range(-maxRange, maxRange);
        Debug.Log("maxRange " + maxRange + ", dropLocationX " + dropLocationX.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // destroy carrier if goes out of scene
        if (transform.position.x > GameObject.Find("Global").GetComponent<Global>().horizontalBound + 5f)
        {
            gameObject.active = false;
            Destroy(this);
        }
        else if (!dropped && transform.position.x > dropLocationX)
        {
            dropped = true;
            DropResource();
        }
        transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
    }

    void DropResource()
    {
        int resourceTypeVal = Random.Range(0, System.Enum.GetNames(typeof(ResourceType)).Length);
        ResourceType resourceType = (ResourceType) System.Enum.GetValues(typeof(ResourceType)).GetValue(resourceTypeVal);
        Resource resource;
        if (resourceType == ResourceType.InvaderSlowDown)
        {
            resource = Instantiate(rInvaderSlowDownPrefab, transform.position, Quaternion.identity).GetComponent<Resource>();
        } else if (resourceType == ResourceType.ShipFaster)
        {
            resource = Instantiate(rShipFasterPrefab, transform.position, Quaternion.identity).GetComponent<Resource>();
        } else
        {
            resource = Instantiate(rInvaderBulletSlowDownPrefab, transform.position, Quaternion.identity).GetComponent<Resource>();
        }
        resource.type = resourceType;
    }
}
