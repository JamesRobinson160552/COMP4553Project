using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpell : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            GetComponent<SpellBase>().setPlayerAccess();
            GameManager.i.MenuText.SetMenu();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            ShowTutorialDisplay();
        }
    }

    public void ShowTutorialDisplay()
    {
        GetComponentInParent<SpellAcquiredDisplayer>().SetDisplayMenu(GetComponent<SpellBase>());
    }
}