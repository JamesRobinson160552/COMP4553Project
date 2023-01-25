using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    int damage;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public int getDamage()
    {
        return damage;
    }

    private void Update() {
        onCollisionEnter();
    }

    public void onCollisionEnter() 
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.BorderLayer);
        if (collider != null) //Destory on collision with border
        {
            Destroy(gameObject);
        }
    }
}
