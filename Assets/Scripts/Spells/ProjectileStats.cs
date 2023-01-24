using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    int damage;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public int getDamage()
    {
        return damage;
    }
}
