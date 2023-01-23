using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    // All space will be a child class of Spell Base
    // All spells will have the following attributes:
    // - Name (string)
    // - Activation sequence  = eg. public List<char> spellActivate = new List<char> {'A', 'A', 'A', 'A' };
    // - Image (the icon to be loaded in the UI when spell is prepped
    // - Script component for specific spell
    // - Cooldown
    // - More to come
    // public string name;
    //public List<char> spellActivate;
    //public void castSpell()
    //{
    // Implemented in derived class
    //}

    // This base class will include arrays of:
    // - spells that the player has access to
    // - all spells in the game

    //public WallSpellScript wallSpell;
    public WallSpellScript wallSpell;
}
