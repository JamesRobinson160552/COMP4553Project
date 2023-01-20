using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] Autoattack attack;
    [SerializeField] List<char> Spell1;
    [SerializeField] List<char> Spell2;

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
    public void CheckForSpells()
    {
        if(ComparingList(Spell1))
        {
            attack.SpecialShoot();
            Debug.Log("Special Spell"); //call diff shooting scripts here
        }
        else if(ComparingList(Spell2))
        {
            Debug.Log("Special Spel2"); //call diff shooting scripts here
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
