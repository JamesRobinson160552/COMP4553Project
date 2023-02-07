using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LightningSpellScript : MonoBehaviour, SpellBase
{
    public List<char> spellActivate = new List<char> { 'D', 'A', 'D', 'W' };
    public string spellName = "Lightning";
    public int damage;
    public GameObject[] spellPrefabs;
    public GameObject plr;
    public GameManager gameManager;
    private float castLoop = 0.0f;
    private float castTime = 1.25f;
    private float BaseTime;
    private bool lightningCast = false;
    private bool BaseTimeSet = false;
    private GameObject lightning;


    public string getName()
    { return spellName; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return damage; }

    public void castSpell()
    {
        if (gameManager.lightningSpawned == false)
        {
            lightningCast = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            lightning = Instantiate(spellPrefabs[0], mousePos, spellPrefabs[0].transform.rotation);
            lightning.GetComponent<ProjectileStats>().SetDamage(0);
        }
    }

    public void LateUpdate()
    {
        if (lightningCast == true)
        {
            gameManager.lightningSpawned = true;
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
                    try
                    {
                        Destroy(lightning);
                    }
                    catch { }
                    lightningCast = false;
                    BaseTimeSet = false;
                    castLoop = 0.0f;
                    gameManager.lightningSpawned = false;
                } else {
                    lightning.GetComponent<Renderer>().material.color = Color.red;
                    lightning.gameObject.layer = LayerMask.NameToLayer("Lightning");
                    //damage = 2;
                    lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                }
            }            
        }
    }

    public void onCollisionEnter()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.BorderLayer);
        if (collider != null) //Destory on collision with border
        {
            Debug.Log("COLLISION!");
        }
    }
}