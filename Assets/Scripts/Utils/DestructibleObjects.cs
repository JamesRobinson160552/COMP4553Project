using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField] int MaxHP;
    [SerializeField] float radius;
 
    
    
    int damage_;
    int currentHP_;

    void Start()
    {
        currentHP_ = MaxHP;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        var collider = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.PlayerSpellsLayer));
        if(collider != null)
        {
            damage_ = collider.GetComponent<ProjectileStats>().getDamage();
            currentHP_ -= damage_;
            CameraShake.i.StopShake();
            Destroy(collider.gameObject); //destroy projectile  
        }

        collider = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
        if(collider != null)
        {
            damage_ = collider.GetComponent<ProjectileStats>().getDamage();
            currentHP_ -= damage_;
        }

        if(currentHP_ <= MaxHP/2)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if(currentHP_ <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
