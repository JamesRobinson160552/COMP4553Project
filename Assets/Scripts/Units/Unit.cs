using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitBase unitBase_;
    public string drop;

    public int currentHP { get; set;}

    public HealthBar healthBar; // Link UI health bar
    //public int maxHealth = 100; // Testing
    public UnitBase GetUnitBase { get { return unitBase_ ;} }
    float hitbox;
    public float radius = 0.8f;
    public float hitcoolDown;
    public float blinkSpeed = 0.05f;
    float timer;
    float timer2;
    bool dying;
    int frames = 0;
    GameObject dropObject;
    [SerializeField] AudioSource playerHitAudio;
    [SerializeField] GameManager gameManager;

    List<SpriteRenderer> SpriteRenderers_ = new List<SpriteRenderer>();

    void Start()
    {
        currentHP = unitBase_.MaxHP;
        hitbox = unitBase_.HitBoxMultiplier;
        GetComponentsInChildren(SpriteRenderers_);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(drop != "")
        {
            //try{
                dropObject = GameObject.Find(drop);
                if(dropObject.GetComponent<SpellBase>().playerHasAccess() == false)
                    dropObject.SetActive(false);
            //}
            //catch{}
        }

        if(healthBar != null)
            healthBar.SetMaxHealth(currentHP);  // In health bar, set at max to begin
    }

    private void FixedUpdate()
    {
        if(dying == true)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x/1.3f, gameObject.transform.localScale.y/1.3f, 0);
            frames++;
            if(frames == 25)
            {
                Destroy(gameObject);
                frames = 0;
                GameManager.i.enemiesKilled++;
            }
        }
        if(timer <= 0)
        {
            for(int i = 0; i < SpriteRenderers_.Count; i++)
            {
                SpriteRenderers_[i].enabled = true; //turn on sprite renderers
            }

            if(unitBase_.Name != "Plague" && !dying)
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
                        for(int i = 0; i < SpriteRenderers_.Count; i++)
                        {
                            SpriteRenderers_[i].enabled = false;
                        }
                    }
                    catch
                    {

                    }
                    CheckForDeath(); //check if unit died
                }
            }

            if((unitBase_.Name == "Plague") && (GameManager.i.gameActive == true))
            {
                var collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.EnemySpellsLayer);
                var collider2 = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
                if((collider != null) || (collider2 != null))
                {
                    timer = hitcoolDown;
                    int damage;
                    damage = tryGetDamage();
                    TakeDamage(damage);
                    playerHitAudio.Play();
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
                    playerHitAudio.Play();
                }
            }
        }
        else
        {
            if(timer2 <= 0)
            {
                for(int i = 0; i < SpriteRenderers_.Count; i++)
                {
                    SpriteRenderers_[i].enabled = !SpriteRenderers_[i].enabled;
                }
                timer2 = blinkSpeed;
            }
        }

        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
    }

    public void bossDamage(int damage)
    {
        if(timer <= 0)
        {
            timer = hitcoolDown;
            TakeDamage(damage);
            playerHitAudio.Play();
        }
    }

    void CheckForDeath()
    {
        if(currentHP <= 0 && unitBase_.Name != "Plague") //Player respawns rather than being destroyed 
        {
            if (unitBase_.Name == "Molten Man")
            {
                gameManager.EndGame();
            }

            if(dropObject != null)
            {
                //try{
                    if(dropObject.GetComponent<SpellBase>().playerHasAccess() == false)
                    {
                        dropObject.SetActive(true);
                        dropObject.transform.position = transform.position;
                    }
                //}
                //catch {}
            }
            dying = true;
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
