using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitBase unitBase_;

    public int currentHP { get; set;} 

    void Start()
    {
        currentHP = unitBase_.MaxHP;
        Debug.Log(unitBase_.Name);
    }
}
