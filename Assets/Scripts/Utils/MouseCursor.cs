using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D blastCursor;
    public Texture2D healCursor;
    public Texture2D lightingCursor;
    public Texture2D wallCursor;
    public Texture2D reflectCursor;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = new Vector2(30f,30f);
    //public Vector2 hotSpot = Vector2.zero;

    SpellController spellController;
    private string spellName;

    void Start()
    {
        Cursor.SetCursor(normalCursor, hotSpot, cursorMode);
    }

    public void Awake()
    {
        spellController = GameObject.Find("Player").GetComponent<SpellController>();
    }


    // Update is called once per frame
    void Update()
    {        
        //Vector2 cursorPos 
        spellName = spellController.spellName;
        if (spellName == "")
        {
            Cursor.SetCursor(normalCursor, hotSpot, cursorMode);
        }
        else if (spellName == "Blast")
        {
            Cursor.SetCursor(blastCursor, hotSpot, cursorMode);
        }
        else if (spellName == "Heal")
        {
            Cursor.SetCursor(healCursor, hotSpot, cursorMode);
        }
        else if (spellName == "Lightning")
        {
            Cursor.SetCursor(lightingCursor, hotSpot, cursorMode);
        }
        else if (spellName == "Wall")
        {
            Cursor.SetCursor(wallCursor, hotSpot, cursorMode);
        }
        else if (spellName == "Reflect")
        {
            Cursor.SetCursor(reflectCursor, hotSpot, cursorMode);
        }
    }
}
