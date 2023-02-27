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
    public bool playerAccess;
    public GameObject[] spellPrefabs;
    public GameObject plr;
    public GameManager gameManager;
    private float castLoop = 0.0f;
    private float castTime = 1.25f;
    private float BaseTime;
    private bool lightningCast = false;
    private bool BaseTimeSet = false;
    private GameObject lightning;
    public string desc;
    SpriteRenderer[] lightningSprite;
    float distanceAboveMouse = 12f;
    int counter =0;
    Vector3 mousePos;

    public string getName()
    { return spellName; }

    public bool playerHasAccess()
    { return playerAccess; }

    public string getDesc()
    {
        return desc;
    }

    public void setPlayerAccess()
    { playerAccess = !playerAccess; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return damage; }

    public void castSpell()
    {
        if (gameManager.lightningSpawned == false)
        {
            counter = 0;
            lightningCast = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            //spritePostion = new Vector3(mousePos.x, mousePos.y + 10f, 0);
            lightning = Instantiate(spellPrefabs[0], mousePos, spellPrefabs[0].transform.rotation);
            lightningSprite = lightning.GetComponentsInChildren<SpriteRenderer>();
            lightningSprite[1].enabled = false; //0 is the circle, 1 is the lightning sprite
            //lightningSprite[1].transform.position = new Vector3(mousePos.x, mousePos.y + distanceAboveMouse, 0);
            lightning.GetComponent<ProjectileStats>().SetDamage(0);
            lightning.GetComponent<ProjectileStats>().SetDestructTimer(castTime + 0.2f);
            lightning.GetComponent<ProjectileStats>().CauseCameraShake(true, true, 0.01f);
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
                    if(lightning !=null )
                        lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    lightningCast = false;
                    BaseTimeSet = false;
                    castLoop = 0.0f;
                    gameManager.lightningSpawned = false;
                } else {
                    if (lightning != null ) //this gets run 9 times, go down for 2 frames and then do damage for the rest
                    {
                        counter++;
                        //Debug.Log(counter);
                        lightningSprite[1].enabled = true;
                        if(counter < 4)
                            lightningSprite[1].transform.position = new Vector3(mousePos.x, mousePos.y + distanceAboveMouse/counter, 0);
                        if(counter == 4)
                            lightning.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                        //lightningSprite[1].material.color = Color.red;
                        lightning.GetComponent<Renderer>().enabled = false;
                        lightning.gameObject.layer = LayerMask.NameToLayer("Lightning");
                        //damage = 2;
                        lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    }
                }
            }            
        }
    }
}