using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public GameObject blackoutScreen;

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Teleported");
            blackoutScreen.gameObject.SetActive(true);
            collision.gameObject.transform.position = new Vector3(-1.4f, -97.0f, 0.0f);
            yield return new WaitForSeconds(0.5f);
            blackoutScreen.gameObject.SetActive(false);
        }
    }
}
