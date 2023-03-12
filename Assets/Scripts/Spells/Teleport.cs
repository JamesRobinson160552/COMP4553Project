using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public FadePanel fadeScreen;
    public GameManager gameManager;
    public GameObject crow;
    public Vector3 bossRoomSpawn = new Vector3(-28f, -65.0f, 0.0f);

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Teleported");
            gameManager.gameActive = false;
            fadeScreen.FadeIn(2f);
            yield return new WaitForSeconds(3f);
            collision.gameObject.transform.position = bossRoomSpawn;
            gameManager.StartBossMusic();
            GameManager.i.reachedBoss = true;
            crow.transform.position = bossRoomSpawn;
            yield return new WaitForSeconds(2f);
            fadeScreen.FadeOut(2f);
            gameManager.insideBossRoom = true;
            gameManager.gameActive = true;
        }
    }
}
