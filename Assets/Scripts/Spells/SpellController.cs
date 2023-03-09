using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    [SerializeField] Autoattack attack;
    [SerializeField] SpellCastScript allSpells;
    [SerializeField] Image icon;
    //[SerializeField] List<char> Spell1;

    //public WallSpellScript wallSpell;

    // Create an array that holds all "active" spell and have the CheckForSpells iterate this array
    // When a player picks up a spell, have that spell script be added to the array
    // Create game object for each spell and have it have the script, then you can just add GameObject to the array

    List<SpellBase> spells = new List<SpellBase>();

    List<char> playerInputs_ { get;set; }
    int currentPosition = 0;
    float timer =0;
    bool usedSpecial_;

    public void Awake()
    {
        //intilize list
        playerInputs_ = new List<char>()
        {
            'Z', 'Z', 'Z', 'Z'
        };
        usedSpecial_ = false;
    }

    public void Start()
    {
        //gets all children of ALL SPELLS game object, and puts them in a list
        allSpells.GetComponentsInChildren(spells);
        //Debug.Log(spells.Count);
    }

    //changing values of list to match play inputs
    public void AddToSpellCommand(char c)
    {
        if(currentPosition < 4)
        {
            playerInputs_[currentPosition] = c;
            currentPosition++;
            Debug.Log(c); 
            CheckForSpellsIcons();
        }
    }

    public void CheckForSpellsIcons()
    {
        for(int i = 0; i < spells.Count; i++)
        {
            if(ComparingList(spells[i].getSpellActivate()) && spells[i].playerHasAccess()) 
            {
                usedSpecial_ = true;
                Debug.Log(spells[i].getName());
                icon.sprite = spells[i].getIcon();  // calls the castSpell script from the spell itself  
            }

            else
            {
                icon.sprite = attack.getIcon();
            }
        }
    }

    //check player list against list for spells
    public void CheckForSpells() //Should take in array of spells plr has access to
    {

        // for every item in array of spells
        for(int i = 0; i < spells.Count; i++)
        {
            if(ComparingList(spells[i].getSpellActivate()) && spells[i].playerHasAccess()) 
            {
                usedSpecial_ = true;
                Debug.Log(spells[i].getName());
                spells[i].castSpell();  // calls the castSpell script from the spell itself  
            }
        }
        
        if(usedSpecial_ == false)
        {
            timer = 0.25f;
            attack.castSpell();
            Debug.Log("basic spell");
            icon.sprite = attack.getIcon();
        }
        
        resetPlayerInputs();
    }

    public void FixedUpdate()
    {
        if(timer > 0)
        {
            GetComponent<PlayerController>().currentMoveSpeed = GetComponent<PlayerController>().moveSpeed/2;
        }

        else
        {
            GetComponent<PlayerController>().currentMoveSpeed = GetComponent<PlayerController>().moveSpeed;
        }

        timer -= Time.fixedDeltaTime;
    }

    //we do a lot of comparing list so made a function to keep things clean
    public bool ComparingList(List<char> spell)
    {
        for(int i = 0; i < 4; i++)
        {
            if(playerInputs_[i] != spell[i])
                return false;
        }
        return true;
    }

    //resets the player inputs back to the defaults
    public void resetPlayerInputs()
    {
        usedSpecial_= false;
        for(int i = 0; i < 4; i++)
        {
            playerInputs_[i] = 'Z';
        }
        currentPosition = 0;
    }
}
