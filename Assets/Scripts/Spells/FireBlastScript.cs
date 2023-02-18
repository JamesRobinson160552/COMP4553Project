using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastScript : MonoBehaviour, SpellBase
{
    public Transform aimer;
    public GameObject[] spellPrefabs;
    public bool playerAccess;
    public int damage = 15;
    public string spellName = "Blast";

    public float bulletForce = 5f;

    public List<char> spellActivate = new List<char> {'S', 'S', 'S', 'W' };

    public string getName()
    { return spellName; }

    public string getDesc()
    { return ""; }

    public bool playerHasAccess()
    { return playerAccess; }

    public void setPlayerAccess()
    { playerAccess = !playerAccess; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return damage; }

    public void castSpell()
    {
        // Instantiates bullet at location of aimer
        GameObject bullet = Instantiate(spellPrefabs[0], aimer.position, aimer.rotation);

        // Access the bullet's rigidbody and store it as rb
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //give the projectile the stats from the sepll
        bullet.GetComponent<ProjectileStats>().SetDamage(damage);
        bullet.GetComponent<ProjectileStats>().CauseCameraShake(true, false, 0.5f);
        bullet.GetComponent<ProjectileStats>().SetDestructTimer(2f);

        // Add force to the newly instantiated rb
        rb.AddForce(aimer.up * bulletForce, ForceMode2D.Impulse);
    }
}
