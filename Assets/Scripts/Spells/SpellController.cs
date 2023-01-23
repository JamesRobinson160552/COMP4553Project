using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] Autoattack attack;
    //[SerializeField] SpellCastScript spellLists;
    //[SerializeField] List<char> Spell1;

    public WallSpellScript wallSpell;

    // Create an array that holds all "active" spell and have the CheckForSpells iterate this array
    // When a player picks up a spell, have that spell script be added to the array
    // Create game object for each spell and have it have the script, then you can just add GameObject to the array



    List<char> playerInputs_ { get;set; }
    int currentPosition = 0;

    public void Awake()
    {
        //intilize list
        playerInputs_ = new List<char>()
        {
            'Z', 'Z', 'Z', 'Z'
        };
    }

    //changing values of list to maatch play inputs
    public void AddToSpellCommand(char c)
    {
        if(currentPosition < 4)
        {
            playerInputs_[currentPosition] = c;
            currentPosition++;
            Debug.Log(c); 
        }
    }

    //check player list against list for spells
    public void CheckForSpells() //Should take in array of spells plr has access to
    {

        // for every item in array of spells
           
        if(ComparingList(wallSpell.spellActivate)) // change to i.spellActivate
        {
            Debug.Log(wallSpell.spellName); //call diff shooting scripts here
            wallSpell.castSpell();  // calls the castSpell script from the spell itself   // change to i.castSpell();
            //spellLists.castWall();

        }
        else
        {
            attack.Shoot();
            Debug.Log("basic spell");
        }
        
        resetPlayerInputs();
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
        for(int i = 0; i < 4; i++)
        {
            playerInputs_[i] = 'Z';
        }
        currentPosition = 0;
    }
}
