using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroys the wall created after a given length of time;
public class WallSpellScript : SpellBase
{

    private int lifeSpanFrames = 3600; //Note: 60fps
    public int lifeRemaining;
    public List<char> spellActivate = new List<char> {'A', 'A', 'A', 'A' };
    public string spellName = "Wall";
    public GameObject[] spellPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        lifeRemaining = lifeSpanFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeRemaining == 0)
        {
            Destroy(gameObject);
        }
        lifeRemaining--;
    }
    public void castSpell()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition;
        position = new Vector3(position.x, position.y, 0);
        Debug.Log(position);

        Instantiate(spellPrefabs[0], position, spellPrefabs[0].transform.rotation); // 
    }
}
