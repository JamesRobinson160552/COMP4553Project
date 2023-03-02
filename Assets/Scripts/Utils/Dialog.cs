using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialog 
{
    //stores the dialog and returns it
    public List<string> lines;

    public List<string> Lines{
        get { return lines; }
    }
}
