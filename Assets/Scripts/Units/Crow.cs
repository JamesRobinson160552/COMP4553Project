using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crow : MonoBehaviour
{
    [SerializeField] Sprite portrait;
    [SerializeField] Dialog dialog;
    [SerializeField] List<Sprite> spaceFrames;
    [SerializeField] SpriteRenderer spaceRenderer;
    [SerializeField] List<string> helpDialog;
    [SerializeField] List<string> basicDialog;
    [SerializeField] List<string> afterLightningDialog;
    [SerializeField] List<string> afterReflectDialog;
    [SerializeField] List<string> afterWallDialog;
    [SerializeField] List<string> afterBlastDialog;
    [SerializeField] List<string> introDialog;
    [SerializeField] List<string> firstTimeSeeingCave;
    [SerializeField] List<string> afterKeyPickup;

    [SerializeField] AudioSource voice;

    Transform playerPos_;
    Character character_;
    Vector2 direction_;
    Vector3 target_;
    Vector3 crowPositon;
    Rigidbody2D rb;
    bool newDialog;
    float timer;
    bool talkable;
    //Dialog dialog;

    float moveSpeed;

    void Start()
    {
        playerPos_ = GameObject.Find("Player").transform;
        character_ = GetComponent<Character>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        dialog.lines = basicDialog;
    }

    void Update()
    {
        target_ = new Vector3(playerPos_.position.x, playerPos_.position.y + 1.7f, playerPos_.position.z);
        moveSpeed = GameObject.Find("Player").GetComponent<PlayerController>().currentMoveSpeed - 0.2f;
        direction_ = (target_ - transform.position).normalized;

        if(GameManager.i.playLightningDialog || GameManager.i.playReflectDialog || GameManager.i.playWallDialog || GameManager.i.playKeyDialog || GameManager.i.playBlastDialog)
        {
            newDialog = true;
        }

        timer -= Time.deltaTime;

        if(newDialog && talkable)
        {
            spaceRenderer.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

            if(timer <= 0 && ( spaceRenderer.sprite == spaceFrames[1] || (spaceRenderer.enabled == false)))
            {
                spaceRenderer.enabled = true;
                spaceRenderer.sprite = spaceFrames[0];
                timer = 0.5f;
            }
            if(timer <= 0 && spaceRenderer.sprite == spaceFrames[0])
            {
                spaceRenderer.sprite = spaceFrames[1];
                timer = 0.5f;
            }
        }

        else
        {
            spaceRenderer.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if(!GameManager.i.gameActive)
        {
            rb.velocity = new Vector2 (0,0) * 0;
            talkable = false;
        }

        else if(((transform.position.x - 0.1f < target_.x && target_.x < transform.position.x + 0.1f) && (transform.position.y - 0.1f < target_.y && target_.y < transform.position.y + 0.1f)) || (!GameManager.i.leftStartingZone) || (GameManager.i.showingDialog))
        {
            character_.Animator.ChangeIsMoving(false);
            rb.velocity = new Vector2(0, 0) * 0;
            talkable = true;
        }

        else
        {
            rb.velocity = new Vector2(direction_.x, direction_.y) * moveSpeed;
            character_.Moving(direction_);
            talkable = false;
        }
    }

    public IEnumerator TalkToCrow(bool forceTalkable = false)
    {
        if(GameManager.i.showingDialog == false && (talkable == true) || (forceTalkable == true)) 
        {
            voice.Play();

            if(GameManager.i.playLightningDialog)
            {
                GameManager.i.playLightningDialog = false;
                dialog.lines = afterLightningDialog;
            }

            else if(GameManager.i.playWallDialog)
            {
                GameManager.i.playWallDialog = false;
                dialog.lines = afterWallDialog;
            }

            else if(GameManager.i.playReflectDialog)
            {
                GameManager.i.playReflectDialog = false;
                dialog.lines = afterReflectDialog;
            }

            else if(GameManager.i.playIntroductionDialog)
            {
                GameManager.i.playIntroductionDialog = false;
                dialog.lines = introDialog;
                GameManager.i.enemiesKilled++;
            }

            else if(GameManager.i.playBlastDialog)
            {
                GameManager.i.playBlastDialog = false;
                dialog.lines = afterBlastDialog;
            }

            else if(GameManager.i.playKeyDialog)
            {
                GameManager.i.playKeyDialog = false;
                dialog.lines = afterKeyPickup;
            }

            else if(GameManager.i.playCaveIntroDialog)
            {
                GameManager.i.playCaveIntroDialog = false;
                dialog.lines = firstTimeSeeingCave;
            }

            else if(!GameManager.i.leftStartingZone)
            {
                dialog.lines = helpDialog;
            }

            else
            {
                dialog.lines = basicDialog;
            }

            GameManager.i.showingDialog = true;
            yield return DialogManager.Instance.ShowDialog(dialog, portrait, dialog.lines.Count);
            newDialog = false;
        }
    }
}
