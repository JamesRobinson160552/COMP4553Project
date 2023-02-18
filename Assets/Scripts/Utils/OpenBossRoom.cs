using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBossRoom : MonoBehaviour
{
    [SerializeField] GameObject bossDoor;

    void OnCollisionEnter()
    {
        Destroy(bossDoor);
    }
}
