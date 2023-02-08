using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpellScript : MonoBehaviour, SpellBase
{
    public float duration = 0f;
    public float timeBetweenHeals = 0.5f;
    public bool playerAccess;
    public List<char> spellActivate = new List<char> {'W', 'S', 'W', 'S' };
    public string spellName = "Heal";
    public HealthBar healthBar;
    public Unit unit;

    float timeRemaining_ = 0f;
    float currentTimeBetweenHeals_ = 0f;

    public string getName()
    { return spellName; }

    public void setPlayerAccess()
    { playerAccess = !playerAccess; }

    public bool playerHasAccess()
    { return playerAccess; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return 2; }

    public void castSpell()
    {
        if(timeRemaining_ <= 0)
            timeRemaining_ = duration;
        GetComponent<SpellAnimator>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void FixedUpdate()
    {
        gameObject.transform.position = unit.gameObject.transform.position;
        
        if(timeRemaining_ > 0 && currentTimeBetweenHeals_ <= 0)
        {
            if(unit.currentHP < unit.GetUnitBase.MaxHP)
            {
                unit.currentHP += getDamage();
                unit.currentHP = Mathf.Clamp(unit.currentHP, 0, unit.GetUnitBase.MaxHP);
                healthBar.setHealth(unit.currentHP);
                Debug.Log(unit.currentHP);
            }
            currentTimeBetweenHeals_ = timeBetweenHeals;
        }
        if(timeRemaining_ <= 0)
        {
            GetComponent<SpellAnimator>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        timeRemaining_ -= Time.deltaTime;
        currentTimeBetweenHeals_ -= Time.deltaTime;

    }
}
