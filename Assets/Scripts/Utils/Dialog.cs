using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialog 
{
    //stores the dialog and returns it
    [SerializeField] List<string> lines;

    public List<string> Lines{
        get { return lines; }
    }
}
