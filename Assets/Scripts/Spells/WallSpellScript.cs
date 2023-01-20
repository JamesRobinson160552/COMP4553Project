using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroys the wall created after a given length of time;
public class WallSpellScript : MonoBehaviour
{

    private int lifeSpanFrames = 360; //Note: 60fps
    public int lifeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        lifeRemaining = lifeSpanFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeRemaining ==0)
        {
            Destroy(gameObject);
        }
        lifeRemaining--;
    }
}
