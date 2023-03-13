using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    int damage;
    bool usesTimer = false;
    float destructTimer;
    bool causesCameraShake;
    bool gradualShake;
    float baseIntensity;
    float currentIntensity;


    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public int getDamage()
    {
        return damage;
    }

    public void SetDestructTimer(float time)
    {
        usesTimer = true;
        destructTimer = time;
    }

    public void CauseCameraShake(bool shakes, bool gradual, float intensity)
    {
        causesCameraShake = shakes;
        gradualShake = gradual;
        baseIntensity = intensity;

    }

    private void FixedUpdate() {

        onCollisionEnter();
        if(causesCameraShake)
        {
            if(gradualShake)
            {
                currentIntensity += baseIntensity;
                CameraShake.i.Shake(currentIntensity);
            }
            else
            {
                CameraShake.i.Shake(baseIntensity);
            }
        }

        if(usesTimer)
        {
            if(destructTimer <= 0)
            {
                CameraShake.i.StopShake();
                Destroy(gameObject);
            }
            destructTimer -= Time.deltaTime;
        }
    }

    public void onCollisionEnter() 
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.BorderLayer);
        if (collider != null) //Destory on collision with border
        {
            usesTimer= false;
            CameraShake.i.StopShake();
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

            if(GameManager.i.insideBossRoom)
            {
                damage = damage/2;
            }

            GetComponent<Rigidbody2D>().velocity =  GetComponent<Rigidbody2D>().velocity * -1;
            GetComponent<SpriteRenderer>().flipY = true;

        }
    }
}
