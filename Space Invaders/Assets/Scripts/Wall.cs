using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int enduranceLeft = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        Debug.Log("Wall");
        if (enduranceLeft > 0)
        {
            enduranceLeft--;
            transform.localScale *= 0.5f;
        } else
        {
            gameObject.active = false;
            Destroy(gameObject);
        }
    }
}
