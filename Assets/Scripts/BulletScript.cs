using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCollisionEnter(Collider other) 
    {
        if (!other.CompareTag("Player")) //Destory on collision with non-player object
        {
            Destroy(gameObject);
        }
    }
}
