using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Current Instructions:
 * To teleport to boss, first go up to talk to bird, then walk back to start location and press "L"
 * To activate boss abilities, press "K"
 * For first 50% of health boss will walk towards you and cast walls to block your projectiles
 *  ToDO: Update wall so it moves with boss, is destructable
 * For last 50% of health, boss will spawn 3 mushrooms in a semi-random pattern once
 *  Boss will cast lightning every 6.5 seconds
 *  ToDO:
 *   Fix boss wall spell which seems to cast from bottom left of his animation
 *   Burn() currently does not do dmg but outputs to Debug.Log. Fix this somehow
 *   Make boss 1.2x as fast on the lower 50% of health
 *   There is a bug where sometimes the lightning stops mid cast and doesn't finish. Sort this out.
 */

public class BossAttacks : MonoBehaviour
{
    public GameObject boss;
    public GameObject plr;

    // Wall Vars
    public GameObject wallPrefab;
    private float lifeSpan = 5.0f;
    public float lifeRemaining;
    private float bossWallBuffer = 6.0f;
    private bool oneWall = false;

    // Lighting Vars
    public GameObject lightningPrefab;
    private bool lightningCast = false;
    private bool BaseTimeSet = false;
    private float BaseTime;
    private float castTime = 1.25f;
    private float castLoop = 0.0f;

    private GameObject lightning0;
    private GameObject lightning1;
    private GameObject lightning2;
    private GameObject lightning3;

    SpriteRenderer[] lightningSprite0;
    SpriteRenderer[] lightningSprite1;
    SpriteRenderer[] lightningSprite2;
    SpriteRenderer[] lightningSprite3;

    Vector3 spawn0;
    Vector3 spawn1;
    Vector3 spawn2;
    Vector3 spawn3;

    public GameObject mushroomPrefab;
    BossAnimator animator_;

    float distanceAboveTarget = 12f;
    int counter;

    // for plrCheatToBoss
    bool onlyOnce = false;

    // Boss Attributes
    int maxHP;
    int currentHP;

    // Boss Logic
    bool enterOnce = true; // Only invoke repeating once
    bool fightTest = false; // Press K to activate the test
    int reset = 1; // amount of times you can reset and go back into the loop


    private void Start()
    {
        animator_ = GetComponent<BossAnimator>();
        lifeRemaining = lifeSpan;
        boss.GetComponent<Unit>().currentHP = 50;
        maxHP = boss.GetComponent<Unit>().currentHP;
    }

    public void Update()
    {
        if (GameManager.i.gameActive == true) {
            // For testing so there is one wall
            if ((Input.GetKey(KeyCode.L)) && (onlyOnce == false)) {
                GameManager.i.insideBossRoom = true;
                plr.transform.position += new Vector3(0, -42, 0); 
                onlyOnce= true;
            }

            burn(); // Burn attack, always active
            currentHP = boss.GetComponent<Unit>().currentHP;
            //Debug.Log(maxHP.ToString());
            //Debug.Log(currentHP.ToString());
            //Debug.Log(reset.ToString());
            if (reset > 0)
            {
                if (currentHP < (maxHP / 2))
                {
                    enterOnce = true;  // When drops below 50% will again enter action loop
                    //Debug.Log("EnterOnce True"); // Testing only
                    reset--;
                }
            }
            //Debug.Log(currentHP.ToString());  

            if (Input.GetKeyDown(KeyCode.K)) fightTest = true; // K is for testing

            if ((fightTest==true) && (enterOnce==true))  
            {           
                if (currentHP >= (maxHP / 2)) // Change to > 50% Max Health
                {
                    InvokeRepeating("shieldUp", 1f, 9f);
                    enterOnce = false;
                }

                if (currentHP < (maxHP / 2))
                {
                    spawnEnemies();
                    InvokeRepeating("randomLightning", 1.5f, 6.5f);
                    // Increase speed to make boss faster
                    enterOnce = false;
                }        
            }

            // Lightning spell specific
            if (lightningCast == true)
            {
                animator_.ChangeIsAttacking(true);
                //gameManager.lightningSpawned = true;
                if (BaseTimeSet == false)
                {
                    BaseTime = Time.realtimeSinceStartup; // Create spell cast start time
                    BaseTimeSet = true;
                }

                // Lightning increases in size until cast time complete
                if (Time.realtimeSinceStartup <= (BaseTime + castTime))
                // if actual time is <= Base + castTime (aka keep going through spell)
                {
                    // If actual time > (base + loop)
                    if (Time.realtimeSinceStartup > (BaseTime + castLoop))
                    {
                        // Make loop bigger and increase size of spell
                        float rnd;
                        castLoop += .1f;
                        if(lightning0 != null)
                        {
                            lightning0.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                            rnd = UnityEngine.Random.Range(.1f, .4f);
                            lightning0.transform.position += new Vector3(-rnd, +rnd, 0);
                        }

                        if(lightning1 != null)
                        {
                            lightning1.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                            rnd = UnityEngine.Random.Range(.1f, .4f);
                            lightning1.transform.position += new Vector3(+rnd, +rnd, 0);
                        }

                        if(lightning2 != null)
                        {
                            lightning2.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                            rnd = UnityEngine.Random.Range(.1f, .4f);
                            lightning2.transform.position += new Vector3(-rnd, -rnd, 0);
                        }

                        if(lightning3 != null)
                        {
                            lightning3.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                            rnd = UnityEngine.Random.Range(.1f, .4f);
                            lightning3.transform.position += new Vector3(+rnd, -rnd, 0);
                        }
                    }
                }

                // Lightning turns red and does dmg
                if (Time.realtimeSinceStartup > (BaseTime + castTime))
                {
                    // Last part of code
                    if (Time.realtimeSinceStartup > (BaseTime + castTime + .1f))
                    {
                        animator_.ChangeIsAttacking(false);

                        if (lightning0 != null)
                        {
                            lightningCast = false;
                            BaseTimeSet = false;
                            castLoop = 0.0f;
                            Destroy(lightning0, 0.0f);
                        }

                        if (lightning1 != null)
                        {
                            lightningCast = false;
                            BaseTimeSet = false;
                            castLoop = 0.0f;
                            Destroy(lightning1, 0.0f);
                        }

                        if (lightning2 != null)
                        {
                            lightningCast = false;
                            BaseTimeSet = false;
                            castLoop = 0.0f;
                            Destroy(lightning2, 0.0f);
                        }

                        if (lightning3 != null)
                        {
                            lightningCast = false;
                            BaseTimeSet = false;
                            castLoop = 0.0f;
                            Destroy(lightning3, 0.0f);
                        }
                            //damage = 2;
                            // Safety check 
                            //lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                        
                        //gameManager.lightningSpawned = false;
                    } else { // Change colour to red

                        counter++;

                        if (lightning0!= null)
                        {
                            lightningSprite0[1].material.color = Color.red;
                            lightningSprite0[1].enabled = true;
                            if(counter < 4)
                                lightningSprite0[1].transform.position = new Vector3(lightning0.transform.position.x, lightning0.transform.position.y + distanceAboveTarget/counter, 0);

                            if(counter == 4)
                                lightning0.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                            if(counter == 8)
                                lightning0.GetComponentInChildren<SpellAnimator>().playEndFrames = true;
                            //lightning0.GetComponent<Renderer>().material.color = Color.blue;
                            lightning0.GetComponent<Renderer>().enabled = false;
                            lightning0.gameObject.layer = LayerMask.NameToLayer("Lightning");
                        }

                        if (lightning1!= null)
                        {
                            lightningSprite1[1].material.color = Color.red;
                            lightningSprite1[1].enabled = true;
                            if(counter < 4)
                                lightningSprite1[1].transform.position = new Vector3(lightning1.transform.position.x, lightning1.transform.position.y + distanceAboveTarget/counter, 0);

                            if(counter == 4)
                                lightning1.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                            if(counter == 8)
                                lightning1.GetComponentInChildren<SpellAnimator>().playEndFrames = true;
                            //lightning1.GetComponent<Renderer>().material.color = Color.blue;
                            lightning1.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning1.GetComponent<Renderer>().enabled = false;
                        }

                        if (lightning2!= null)
                        {
                            lightningSprite2[1].material.color = Color.red;
                            lightningSprite2[1].enabled = true;
                            if(counter < 4)
                                lightningSprite2[1].transform.position = new Vector3(lightning2.transform.position.x, lightning2.transform.position.y + distanceAboveTarget/counter, 0);

                            if(counter == 4)
                                lightning2.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                            if(counter == 8)
                                lightning2.GetComponentInChildren<SpellAnimator>().playEndFrames = true;
                            //lightning2.GetComponent<Renderer>().material.color = Color.blue;
                            lightning2.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning2.GetComponent<Renderer>().enabled = false;
                        }

                        if (lightning3!= null)
                        {
                            lightningSprite3[1].material.color = Color.red;
                            lightningSprite3[1].enabled = true;
                            if(counter < 4)
                                lightningSprite3[1].transform.position = new Vector3(lightning3.transform.position.x, lightning3.transform.position.y + distanceAboveTarget/counter, 0);

                            if(counter == 4)
                                lightning3.GetComponentInChildren<SpellAnimator>().playSetUp = false;
                            if(counter == 8)
                                lightning3.GetComponentInChildren<SpellAnimator>().playEndFrames = true;
                            //lightning3.GetComponent<Renderer>().material.color = Color.blue;
                            lightning3.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning3.GetComponent<Renderer>().enabled = false;
                        }
                        //damage = 2;
                        //lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    }
                }
            }
        }
    }

    void spawnEnemies()
    {   
        Vector3 plrPos = new Vector3(plr.transform.position.x, plr.transform.position.y, plr.transform.position.z);
        Vector3 bossPos = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        Vector3 halfDif = new Vector3(((bossPos.x + plrPos.x) / 2), ((bossPos.y + plrPos.y) / 2), ((bossPos.z + plrPos.z) / 2));
        
        // Create mushroom 1 at std pos
        GameObject mushroom1 = Instantiate(mushroomPrefab, halfDif, mushroomPrefab.transform.rotation);

        // Create mushroom 2 at rnd offset
        float rnd = UnityEngine.Random.Range(.1f, 1.4f); 
        Vector3 offset = new Vector3(rnd, rnd, 0);
        GameObject mushroom2 = Instantiate(mushroomPrefab, (halfDif + offset), mushroomPrefab.transform.rotation);

        // Create mushroom 3 at -rnd offset
        rnd = UnityEngine.Random.Range(.1f, 1.4f);
        offset = new Vector3(rnd, rnd, 0);
        GameObject mushroom3 = Instantiate(mushroomPrefab, (halfDif - offset), mushroomPrefab.transform.rotation);
    }

    void randomLightning()
    {
        int rnd = UnityEngine.Random.Range(3, 6);
        Vector3 bossPos = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        Vector3 lightningBaseLoc0 = new Vector3(bossPos.x + rnd, bossPos.y + rnd, bossPos.z); // (+,+)
        Vector3 lightningBaseLoc1 = new Vector3(bossPos.x + rnd, bossPos.y - rnd, bossPos.z); // (+,-)
        Vector3 lightningBaseLoc2 = new Vector3(bossPos.x - rnd, bossPos.y + rnd , bossPos.z); // (-,+)
        Vector3 lightningBaseLoc3 = new Vector3(bossPos.x - rnd, bossPos.y - rnd, bossPos.z); // (-,-)

        // Spawn 4 lightnings
        lightning0 = Instantiate(lightningPrefab, lightningBaseLoc0, lightningPrefab.transform.rotation);
        lightning1 = Instantiate(lightningPrefab, lightningBaseLoc1, lightningPrefab.transform.rotation);
        lightning2 = Instantiate(lightningPrefab, lightningBaseLoc2, lightningPrefab.transform.rotation);
        lightning3 = Instantiate(lightningPrefab, lightningBaseLoc3, lightningPrefab.transform.rotation);

        lightningSprite0 = lightning0.GetComponentsInChildren<SpriteRenderer>();
        lightningSprite1 = lightning1.GetComponentsInChildren<SpriteRenderer>();
        lightningSprite2 = lightning2.GetComponentsInChildren<SpriteRenderer>();
        lightningSprite3 = lightning3.GetComponentsInChildren<SpriteRenderer>();

        lightningSprite0[1].enabled = false;
        lightningSprite1[1].enabled = false;
        lightningSprite2[1].enabled = false;
        lightningSprite3[1].enabled = false;

        spawn0 = lightning0.transform.position;
        spawn1 = lightning1.transform.position;
        spawn2 = lightning2.transform.position;
        spawn3 = lightning3.transform.position;

        counter = 0;

        // Go through proper lighting spell cast
        lightningCast = true;
    }

    void burn()
    {
        Vector3 plrPos = new Vector3(plr.transform.position.x, plr.transform.position.y, plr.transform.position.z);
        Vector3 bossPos = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        Vector3 distance = new Vector3(plrPos.x - bossPos.x, plrPos.y - bossPos.y, plrPos.z - bossPos.z);
        if (distance.magnitude < 4)
        {
            plr.GetComponent<Unit>().bossDamage(5);
        } else { }
    }

    void shieldUp()
    {
        // Base vectors
        Vector3 plrPos = new Vector3(plr.transform.position.x, plr.transform.position.y, plr.transform.position.z);
        Vector3 bossPos = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        Vector3 wallDif = new Vector3(((bossPos.x + plrPos.x)/2), ((bossPos.y + plrPos.y) / 2), ((bossPos.z + plrPos.z) / 2));
        //Debug.Log("Plr: " + plrPos);
        //Debug.Log("Boss: " + bossPos);
        //Debug.Log("Wall: " + midDif);

        //Normalize (so wall is close to boss)
        if (wallDif.x > bossWallBuffer) wallDif.x = bossPos.x + bossWallBuffer;
        if (wallDif.x < -bossWallBuffer) wallDif.x = bossPos.x - bossWallBuffer;
        if (wallDif.y > bossWallBuffer) wallDif.y = bossPos.y + bossWallBuffer;
        if (wallDif.y < -bossWallBuffer) wallDif.y = bossPos.y - bossWallBuffer;

        // Angle of wall
        float adjacent = bossPos.x - plrPos.x;
        float opposite = bossPos.y - plrPos.y;
        float angle = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
        Vector3 angleVector = new Vector3(0, 0, angle);

        // Spawn Wall
        GameObject wall = Instantiate(wallPrefab, wallDif, Quaternion.Euler(angleVector));
        wall.GetComponent<Renderer>().material.color = Color.red;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x*2.5f, wall.transform.localScale.y*1.5f, wall.transform.localScale.z);

        wall.GetComponent<DestroyMe>().SetLife(lifeSpan);
    }
}
