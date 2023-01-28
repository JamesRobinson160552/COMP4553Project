using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuText : MonoBehaviour
{
    [SerializeField] SpellCastScript allSpells;
    [SerializeField] List<TMPro.TextMeshProUGUI> spellsOnScreen;

    List<SpellBase> spells = new List<SpellBase>();

    public void Start()
    {
        //gets all children of ALL SPELLS game object, and puts them in a list
        allSpells.GetComponentsInChildren(spells);
        SetMenu();
    }
 
    void SetMenu()
    {
        for(int i = 0; i < spells.Count; i++)
        {
            spellsOnScreen[i].gameObject.SetActive(true);

            string onScreenText;
            onScreenText = spells[i].getName();
            onScreenText += " : ";

            List<char> tempList = spells[i].getSpellActivate();

            for(int j = 0; j < 4; j++)
            {
                onScreenText += tempList[j];
            }

            spellsOnScreen[i].text = onScreenText;
        }
    }
}
