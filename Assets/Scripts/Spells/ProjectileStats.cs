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

    private void FixedUpdate() {
        onCollisionEnter();
    }

    public void onCollisionEnter() 
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.BorderLayer);
        if (collider != null) //Destory on collision with border
        {
            Destroy(gameObject);
        }

        var collider2 = Physics2D.OverlapCircle(transform.position, 0.3f, GameLayers.i.ReflectLayer);
        if(collider2 != null)
        {
            //on reflect enemy spells become player spells and vice cersa
            if(gameObject.layer == LayerMask.NameToLayer("PlayerSpells"))
            {
                gameObject.layer = LayerMask.NameToLayer("EnemySpells");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerSpells");
            }

            GetComponent<Rigidbody2D>().velocity =  GetComponent<Rigidbody2D>().velocity * -1;
            GetComponent<SpriteRenderer>().flipY = true;

        }
    }
}
