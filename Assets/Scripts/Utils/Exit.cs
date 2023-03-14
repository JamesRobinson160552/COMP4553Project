using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnCollision2DEnter(Collider other)
    {
        if (!GameObject.Find("Boss"))
        {
            gameManager.EndGame();
        }
    }
}
