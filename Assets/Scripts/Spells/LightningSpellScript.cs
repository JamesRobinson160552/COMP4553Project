using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LightningSpellScript : MonoBehaviour, SpellBase
{
    public List<char> spellActivate = new List<char> { 'D', 'A', 'D', 'W' };
    public string spellName = "Lightning";
    public GameObject[] spellPrefabs;
    public GameObject plr;
    private float castLoop = 0.0f;
    private float castTime = 3.0f;
    private float BaseTime;
    private bool lightningCast = false;
    private bool BaseTimeSet = false;
    private GameObject lightning;

    public string getName()
    { return spellName; }

    public List<char> getSpellActivate()
    { return spellActivate; }

    public int getDamage()
    { return 0; }

    public void castSpell()
    {
        lightningCast = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        Debug.Log("Spell cast");
        lightning = Instantiate(spellPrefabs[0], mousePos, spellPrefabs[0].transform.rotation);
        Debug.Log("Spell Instantiated");
    }

    public void LateUpdate()
    {
        if (lightningCast == true)
        {
            if (BaseTimeSet == false)
            {
                BaseTime = Time.realtimeSinceStartup; // Create spell cast start time
                BaseTimeSet = true;
                Debug.Log("BaseTimerSet");
            }

            Debug.Log("BaseTimerSet");
            if (Time.realtimeSinceStartup <= (BaseTime + castTime)) // if actual time is greater Base + castTime
            {
                if (Time.realtimeSinceStartup > (BaseTime + castLoop))
                {
                    Debug.Log(Time.realtimeSinceStartup);
                    Debug.Log(castLoop);
                    castLoop += 1.0f;
                    lightning.transform.localScale = new Vector3(castLoop, castLoop, 1);
                }

                if (castLoop == castTime) // End of Loop
                {
                    lightningCast = false;
                    BaseTimeSet = false;
                    castLoop = 0.0f;
                }
            }
        }
    }
}