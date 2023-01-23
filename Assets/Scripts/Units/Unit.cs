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
        healthBar.SetMaxHealth(currentHP);  // In health bar, set at max to begin
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage; // Unit to take damage

        healthBar.setHealth(currentHP);
    }
}
