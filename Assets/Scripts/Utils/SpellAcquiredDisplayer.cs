using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAcquiredDisplayer : MonoBehaviour
{
    [SerializeField] List<TMPro.TextMeshProUGUI> infoOnScreen;
    [SerializeField] GameObject spellMenu;
 
    public void SetDisplayMenu(SpellBase spell)
    {
        string onScreenText = "";
        infoOnScreen[0].text = spell.getName();

        infoOnScreen[1].text = spell.getDesc();

        List<char> tempList = spell.getSpellActivate();

        for(int i = 0; i < 4; i++)
        {
            onScreenText += tempList[i];
        }

        infoOnScreen[2].text = onScreenText;

        GameManager.i.gameActive = false;

        spellMenu.gameObject.SetActive(true);
    }

    public void TurnOffDisplayMenu()
    {
        GameManager.i.gameActive = true;
        spellMenu.gameObject.SetActive(false);
    }
}
