using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitBase unitBase_;
    [SerializeField] string drop;

    public int currentHP { get; set;}

    public HealthBar healthBar; // Link UI health bar
    //public int maxHealth = 100; // Testing
    public UnitBase GetUnitBase { get { return unitBase_ ;} }
    float hitbox;
    public float radius = 0.8f;
    public float hitcoolDown;
    public float blinkSpeed;
    float timer;
    float timer2;
    GameObject dropObject;

    List<SpriteRenderer> SpriteRenderers_ = new List<SpriteRenderer>();

    void Start()
    {
        currentHP = unitBase_.MaxHP;
        hitbox = unitBase_.HitBoxMultiplier;
        GetComponentsInChildren(SpriteRenderers_);

        if(drop != "")
        {
            dropObject = GameObject.Find(drop);
            dropObject.SetActive(false);
        }

        if(healthBar != null)
            healthBar.SetMaxHealth(currentHP);  // In health bar, set at max to begin
    }

    private void FixedUpdate()
    {
        if(timer <= 0)
        {
            if(unitBase_.Name != "Plague")
            {
                var collider = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.PlayerSpellsLayer));
                var collider2 = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
                if ((collider != null) || (collider2 != null))
                {
                    timer = hitcoolDown;
                    int damage;
                    damage = tryGetDamage();
                    Debug.Log("Damage is: ");
                    Debug.Log(damage);
                    TakeDamage(damage);
                    try
                    {
                        CameraShake.i.StopShake();
                        Destroy(collider.gameObject); //destroy projectile  
                    }
                    catch
                    {

                    }
                    CheckForDeath(); //check if unit died
                }
            }

            else
            {
                SpriteRenderers_[0].enabled = true;
                SpriteRenderers_[1].enabled = true;
                var collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.EnemySpellsLayer);
                var collider2 = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
                if((collider != null) || (collider2 != null))
                {
                    timer = hitcoolDown;
                    int damage;
                    damage = tryGetDamage();
                    TakeDamage(damage);
                    try
                    {
                        CameraShake.i.StopShake();
                        Destroy(collider.gameObject); //destroy projectile  
                    }
                    catch
                    {

                    }
                    CheckForDeath();
                }

                //extended hitbox when checking for physical attack damage
                var collider3 = Physics2D.OverlapCircle(transform.position, radius * hitbox, GameLayers.i.EnemyLayer);
                if(collider3 != null)
                {
                    timer = hitcoolDown;
                    TakeDamage(collider3.GetComponent<Unit>().GetUnitBase.BaseDamage);
                }
            }
        }
        else
        {
            if(timer2 <= 0 && unitBase_.Name == "Plague")
            {
                SpriteRenderers_[0].enabled = !SpriteRenderers_[0].enabled;
                SpriteRenderers_[1].enabled = !SpriteRenderers_[1].enabled;
                timer2 = blinkSpeed;
            }
        }

        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
    }

    void CheckForDeath()
    {
        if(currentHP <= 0 && unitBase_.Name != "Plague") //Player respawns rather than being destroyed 
        {
            if(drop != "")
            {
                dropObject.SetActive(true);
                dropObject.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage; // Unit to take damage

        //Debug.Log("Damage!");
        if(healthBar != null)
            healthBar.setHealth(currentHP);

        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius * hitbox);
    }

    int tryGetDamage() //only gets run when contact is made with spell of opposite faction
    {
        var collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.PlayerSpellsLayer);
        try
        {
            int damage = collider.GetComponent<ProjectileStats>().getDamage(); //get the damage num from projectile
            return damage;
        }
        catch { }

        collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.EnemySpellsLayer);
        try
        {
            int damage = collider.GetComponent<ProjectileStats>().getDamage(); //get the damage num from projectile
            return damage;
        }
        catch { }

        collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer);
        try
        {
            int damage = collider.GetComponent<ProjectileStats>().getDamage(); //get the damage num from projectile
            Debug.Log("Lightning Spell Damage Returned Successfully");
            Debug.Log(damage);
            return damage;
        }
        catch {
            Debug.Log("Lightning spell damage failed");
            return 0; }
    }
}
