using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask playerSpells;
    [SerializeField] LayerMask enemySpells;

    public static GameLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask PlayerSpellsLayer {
        get => playerSpells;
    }

    public LayerMask EnemySpellsLayer {
        get => enemySpells;
    }
}
