using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float lifeSpan;
    
    public void SetLife(float life)
    {
        lifeSpan = life;
    }

    void Update()
    {
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
        lifeSpan -= Time.deltaTime;
    }
}
