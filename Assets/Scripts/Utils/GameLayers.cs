using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask playerSpells;
    [SerializeField] LayerMask enemySpells;
    [SerializeField] LayerMask border;
    [SerializeField] LayerMask reflect;
    [SerializeField] LayerMask interactable;
    [SerializeField] LayerMask lightning;
    [SerializeField] LayerMask enemy;

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

    public LayerMask BorderLayer {
        get => border;
    }

    public LayerMask ReflectLayer {
        get => reflect;
    }

    public LayerMask InteractableLayer {
        get => interactable;
    }

    public LayerMask LightningLayer
    {
        get => lightning;
    }

    public LayerMask EnemyLayer
    {
        get => enemy;
    }
}
