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
    int growth;
    GameObject lightning;
    SpriteRenderer[] lightningSprite;
    float distanceAboveMouse = 12f;
    int counter = 0;
    Vector3 spawn;
    [SerializeField] AudioSource lightningAudio;
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip strikeSound;

    public void SetData(bool cast, int setDamage, float loop, float time, GameObject lightningPrefab, int growthMultiplier=3)
    {
        spawn = transform.position;
        lightningCast = cast;
        damage = setDamage;
        castLoop = loop;
        castTime = time;
        lightning = lightningPrefab;
        lightningSprite = lightning.GetComponentsInChildren<SpriteRenderer>();
        lightning.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
        lightningSprite[1].enabled = false;
        growth = growthMultiplier;
        lightning.GetComponent<ProjectileStats>().CauseCameraShake(true, true, 0.03f);
    }

    public void LateUpdate()
    {
        if (lightningCast == true)
        {
            //GameManager.i.lightningSpawned = true;
            if (BaseTimeSet == false)
            {
                lightningAudio.PlayOneShot(startSound, 0.7f);
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
                    lightning.transform.localScale = new Vector3(castLoop*growth, castLoop*growth, 1);
                }
            }
            if (Time.realtimeSinceStartup > (BaseTime + castTime))
            {
                if (Time.realtimeSinceStartup > (BaseTime + castTime + .1f)) {
                    //damage = 2;
                    lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    lightningAudio.PlayOneShot(strikeSound, 0.5f);
                    lightningCast = false;
                    BaseTimeSet = false;
                    castLoop = 0.0f;
                    //GameManager.i.lightningSpawned = false;
                } else {
                    counter++;
                    lightningSprite[1].enabled = true;
                    if(counter < 4)
                        lightningSprite[1].transform.position = new Vector3(spawn.x, spawn.y + distanceAboveMouse/counter, 0);
                    if(counter == 4)
                        lightning.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                    if(counter == 8)
                        lightning.GetComponentInChildren<SpellAnimator>().playEndFrames = true;
                    lightningSprite[1].material.color = Color.red;
                    lightning.GetComponent<Renderer>().enabled = false;
                    lightning.gameObject.layer = LayerMask.NameToLayer("Lightning");
                    //damage = 2;
                    lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                }
            }            
        }
    }
}
