using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpellBase
{
    // All space will be a child class of Spell Base
    // All spells will have the following attributes:
    // - Name (string)
    string getName();

    bool playerHasAccess();

    void setPlayerAccess();

    int getDamage();
    // - Activation sequence  = eg. public List<char> spellActivate = new List<char> {'A', 'A', 'A', 'A' };
    List<char> getSpellActivate();
    // - Image (the icon to be loaded in the UI when spell is prepped
    //Sprite image { get;set; }
    // - Script component for specific spell
    void castSpell();
    // - Cooldown
    //int cooldown { get;set; }
    // - More to come
}
