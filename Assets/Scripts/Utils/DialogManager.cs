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

    public static DialogManager Instance{ get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowDialog(Dialog dialog, Sprite sprite) //shows dialog
    {
        yield return new WaitForEndOfFrame();

        characterTalking.sprite = sprite;

        GameManager.i.showingDialog = true;

        this.dialog = dialog;

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void Update() //advances text when space bar is hit, or closes text box if all test has been displayed
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isTyping && GameManager.i.showingDialog)
        {
            ++currentLine;
            if(currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                GameManager.i.showingDialog = false;
            }
        }
    }

    public IEnumerator TypeDialog(string line) //types the text letter by letter
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}
