using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitBase unitBase_;

    public int currentHP { get; set;}

    public HealthBar healthBar; // Link UI health bar
    //public int maxHealth = 100; // Testing
    public UnitBase GetUnitBase { get { return unitBase_ ;} }

    public float radius = 0.8f;

    void Start()
    {
        currentHP = unitBase_.MaxHP;
        //Debug.Log(unitBase_.Name);
        if(healthBar != null)
            healthBar.SetMaxHealth(currentHP);  // In health bar, set at max to begin
    }

    private void FixedUpdate()
    {
        if(unitBase_.Name != "Plague")
        {
            var collider = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.PlayerSpellsLayer));
            var collider2 = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
            if ((collider != null) || (collider2 != null))
            {
                int damage;
                damage = tryGetDamage();
                Debug.Log("Damage is: ");
                Debug.Log(damage);
                TakeDamage(damage);
                try
                {
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
            var collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.EnemySpellsLayer);
            var collider2 = (Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.LightningLayer));
            if((collider != null) || (collider2 != null))
            {
                int damage;
                damage = tryGetDamage();
                TakeDamage(damage);
                try
                {
                    Destroy(collider.gameObject); //destroy projectile  
                }
                catch
                {

                }
                CheckForDeath();
            }
        }
    }

    void CheckForDeath()
    {
        if(currentHP <= 0)
        {
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
    }

    int tryGetDamage()
    {
        var collider = Physics2D.OverlapCircle(transform.position, radius, GameLayers.i.PlayerSpellsLayer);
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
