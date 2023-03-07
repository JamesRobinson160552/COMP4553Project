using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpell : MonoBehaviour
{
    public bool lightning;
    public bool reflect;
    public bool wall;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            GetComponent<SpellBase>().setPlayerAccess();
            GameManager.i.MenuText.SetMenu();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            ShowTutorialDisplay();

            if(lightning)
                GameManager.i.playLightningDialog = true;
            if(reflect)
                GameManager.i.playReflectDialog = true;
            if(wall)
                GameManager.i.playWallDialog = true;
        }
    }

    public void ShowTutorialDisplay()
    {
        GetComponentInParent<SpellAcquiredDisplayer>().SetDisplayMenu(GetComponent<SpellBase>());
    }
}
