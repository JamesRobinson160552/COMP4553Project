using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastScript : MonoBehaviour
{

    public GameObject[] spellPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void castWall()
    {
        Vector3 position = Input.mousePosition;
        Instantiate(spellPrefabs[0], position, spellPrefabs[0].transform.rotation);
    }
}
