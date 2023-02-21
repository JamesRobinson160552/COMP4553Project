using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public GameObject boss;
    public GameObject plr;

    // Wall Vars
    public GameObject wallPrefab;
    private float lifeSpan = 5.0f;
    public float lifeRemaining;
    private float bossWallBuffer = 2.5f;
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

    // for plrCheatToBoss
    bool onlyOnce = false;

    private void Start()
    {
        lifeRemaining = lifeSpan;
    }

    public void LateUpdate()
    {
        if (GameManager.i.gameActive == true) {
            // For testing so there is one wall
            if ((Input.GetKey(KeyCode.L)) && (onlyOnce == false)) {
                plr.transform.position += new Vector3(0, -42, 0); 
                onlyOnce= true;
            }

            if ((oneWall == false) && (Input.GetKeyDown(KeyCode.K))) // K is for testing
            {
                randomLightning();
                oneWall = true;
                shieldUp();
            }

            // Lightning spell specific
            if (lightningCast == true)
            {
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
                        castLoop += .1f;
                        lightning0.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                        float rnd = Random.Range(.1f, .4f);
                        lightning0.transform.position += new Vector3(-rnd, +rnd, 0);
                        lightning1.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                        rnd = Random.Range(.1f, .4f);
                        lightning1.transform.position += new Vector3(+rnd, +rnd, 0);
                        lightning2.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                        rnd = Random.Range(.1f, .4f);
                        lightning2.transform.position += new Vector3(-rnd, -rnd, 0);
                        lightning3.transform.localScale = new Vector3(castLoop * 3, castLoop * 3, 1);
                        rnd = Random.Range(.1f, .4f);
                        lightning3.transform.position += new Vector3(+rnd, -rnd, 0);
                    }
                }

                // Lightning turns red and does dmg
                if (Time.realtimeSinceStartup > (BaseTime + castTime))
                {
                    // Last part of code
                    if (Time.realtimeSinceStartup > (BaseTime + castTime + .1f))
                    {
                        if (lightning0 != null)
                        {
                            lightningCast = false;
                            BaseTimeSet = false;
                            castLoop = 0.0f;
                            Destroy(lightning0, 0.0f);
                            Destroy(lightning1, 0.0f);
                            Destroy(lightning2, 0.0f);
                            Destroy(lightning3, 0.0f);
                        }
                            //damage = 2;
                            // Safety check 
                            //lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                        
                        //gameManager.lightningSpawned = false;
                    } else { // Change colour to red
                        if (lightning0!= null)
                        {
                            lightning0.GetComponent<Renderer>().material.color = Color.blue;
                            lightning1.GetComponent<Renderer>().material.color = Color.blue;
                            lightning2.GetComponent<Renderer>().material.color = Color.blue;
                            lightning3.GetComponent<Renderer>().material.color = Color.blue;

                            lightning0.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning1.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning2.gameObject.layer = LayerMask.NameToLayer("Lightning");
                            lightning3.gameObject.layer = LayerMask.NameToLayer("Lightning");
                        }
                        //damage = 2;
                        //lightning.GetComponent<ProjectileStats>().SetDamage(damage);
                    }
                }
            }
        }
    }

    void randomLightning()
    {
        int rnd = Random.Range(3, 6);
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

        // Go through proper lighting spell cast
        lightningCast = true;
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

        wall.GetComponent<DestroyMe>().SetLife(lifeSpan);
    }
}
