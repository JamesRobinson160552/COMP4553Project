using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightning : MonoBehaviour
{
    bool lightningCast;
    int damage;
    bool BaseTimeSet = false;
    float BaseTime;
    float castLoop = 0.0f;
    float castTime = 1.25f;
    GameObject lightning;

    public void SetData(bool cast, int setDamage, float loop, float time, GameObject lightningPrefab)
    {
        lightningCast = cast;
        damage = setDamage;
        castLoop = loop;
        castTime = time;
        lightning = lightningPrefab;
    }

    public void LateUpdate()
    {
        if (lightningCast == true)
        {
            //GameManager.i.lightningSpawned = true;
            if (BaseTimeSet == false)
            {
                BaseTime = Time.realtimeSinceStartup; // Create spell cast start time
                BaseTimeSet = true;
            }

            if (Time.realtimeSinceStartup <= (BaseTime + castTime))
            // if actual time is <= Base + castTime (aka keep going through spell)
            {
                // If actual time > (base + loop)
                if (Time.realtimeSinceStartup > (BaseTime + castLoop))
                {
                    // Make loop bigger and increase size of spell
                    castLoop += .1f;
                    lightning.transform.localScale = new Vector3(castLoop*3, castLoop*3, 1);
                }
            }
            if (Time.realtimeSinceStartup > (BaseTime + castTime))
            {
                if (Time.realtimeSinceStartup > (BaseTime + castTime + .1f)) {
                    //damage = 2;
                    lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    lightningCast = false;
                    BaseTimeSet = false;
                    castLoop = 0.0f;
                    //GameManager.i.lightningSpawned = false;
                } else {
                    lightning.GetComponent<Renderer>().material.color = Color.red;
                    lightning.gameObject.layer = LayerMask.NameToLayer("Lightning");
                    //damage = 2;
                    lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                }
            }            
        }
    }
}
