using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Image characterTalking;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;
    int lines = -1;

    public static DialogManager Instance{ get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowDialog(Dialog dialog1, Sprite sprite1)
    {
        yield return (ShowDialog(dialog1, sprite1, -1));
    }

    public IEnumerator ShowDialog(Dialog dialog, Sprite sprite, int count) //shows dialog
    {
        isTyping = true;
        yield return new WaitForEndOfFrame();

        characterTalking.sprite = sprite;
        lines = count;

        GameManager.i.showingDialog = true;
        GameManager.i.gameActive = false;

        this.dialog = dialog;

        dialogBox.SetActive(true);
        yield return new WaitForEndOfFrame();
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void Update() //advances text when space bar is hit, or closes text box if all test has been displayed
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isTyping && GameManager.i.showingDialog)
        {
            ++currentLine;
            Debug.Log(lines);
            if(lines == -1)
            {
                if(currentLine < dialog.Lines.Count)
                {
                    StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                }

                else
                {
                    currentLine = 0;
                    dialogBox.SetActive(false);
                    GameManager.i.showingDialog = false;
                    GameManager.i.gameActive = true;
                }
            }
            Debug.Log(lines);
            if(lines != -1)
            {
                if(currentLine < lines)
                {
                    StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                }
                else
                {
                    currentLine = 0;
                    dialogBox.SetActive(false);
                    GameManager.i.showingDialog = false;
                    GameManager.i.gameActive = true;
                }
            }
        }
    }

    public IEnumerator TypeDialog(string line) //types the text letter by letter
    {
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}
