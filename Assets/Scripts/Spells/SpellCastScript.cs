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

    /*
    public void castWall()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Input.mousePosition;
        position = new Vector3(position.x, position.y, 0);
        Debug.Log(position);
        Instantiate(spellPrefabs[0], position, spellPrefabs[0].transform.rotation);
    }
    */
}
