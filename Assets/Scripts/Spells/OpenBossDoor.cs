using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Destroys the barrier blocking the boss room when the player touches
public class OpenBossDoor : MonoBehaviour
{

    public GameObject door;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            GameManager.i.playKeyDialog = true;
            GameManager.i.firstTimeSeeingCave = false;
            Destroy(door);
            Destroy(gameObject);
        }
    }
}
