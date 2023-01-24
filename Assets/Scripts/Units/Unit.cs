using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitBase unitBase_;

    public int currentHP { get; set;}

    public HealthBar healthBar; // Link UI health bar
    //public int maxHealth = 100; // Testing
    public UnitBase GetUnitBase { get { return unitBase_ ;} }

    void Start()
    {
        currentHP = unitBase_.MaxHP;
        Debug.Log(unitBase_.Name);
        if(healthBar != null)
            healthBar.SetMaxHealth(currentHP);  // In health bar, set at max to begin
    }

    private void Update()
    {
        if(unitBase_.Name != "Plague")
        {
            var collider = Physics2D.OverlapCircle(transform.position, 0.3f, GameLayers.i.PlayerSpellsLayer);
            if(collider != null)
            {
                int damage = collider.GetComponent<ProjectileStats>().getDamage(); //get the damage num from projectile
                TakeDamage(damage);
                Destroy(collider.gameObject); //destroy projectile
                CheckForDeath(); //check if unit died
            }
        }

        else
        {
            var collider = Physics2D.OverlapCircle(transform.position, 0.3f, GameLayers.i.EnemySpellsLayer);
            if(collider != null)
            {
                int damage = collider.GetComponent<ProjectileStats>().getDamage();
                TakeDamage(damage);
                Destroy(collider.gameObject);
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

        Debug.Log("Damage!");
        if(healthBar != null)
            healthBar.setHealth(currentHP);
    }
}
