using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour, Interactible
{
    [SerializeField] Dialog dialog;
    [SerializeField] Sprite portrait;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenMovement; //how long npc waits before walking again
    [SerializeField] float timeToWalk; //how long npc will walk at one time

    public NPCState state;
    public NPCState prevState;

    float idleTimer = 0f;
    int currentPattern = 0;
    private Character character;

    public float walkSpeed;

    Vector3 targetPOS;
    Vector3 distanceToTarget;

    float walkingTimer; 
    bool justWalked;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;

            character.LookTowards(initator.position);

            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, portrait));
        }
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            if(walkingTimer < 0)
                idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenMovement)
            {
                if (movementPattern.Count > 0)
                    Walk();
                idleTimer = 0;
            }
        }

        if(GameManager.i.showingDialog == true) //keep state in dialog mode while text is up
        {
            state = NPCState.Dialog;
        }

        if(state == NPCState.ClosingDialog) //after seeing bufferstate put NPC but into idle mode
            state = NPCState.Idle; //without the  buffer, an infinite loop of dialog is created

        if((prevState == NPCState.Dialog) && GameManager.i.showingDialog == false) //runs the frame dialog ends
            state = NPCState.ClosingDialog; //this is a buffer state

        if (state != NPCState.Walking) //prevents animation from playing when not moving
            character.Animator.ChangeIsMoving(false);
        else
            character.Animator.ChangeIsMoving(true);

        prevState = state;
        
    }

    public void Walk()
    {
        targetPOS = (transform.position + new Vector3(movementPattern[currentPattern].x, movementPattern[currentPattern].y, 0));
        distanceToTarget = (targetPOS - transform.position).normalized;

        walkingTimer = timeToWalk;
    }

    void FixedUpdate()
    {   
        if(state != NPCState.Dialog)
        {
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            if(walkingTimer > 0)
            {
                state = NPCState.Walking;
                rb.velocity = new Vector2(distanceToTarget.x, distanceToTarget.y) * walkSpeed;
                character.Moving(distanceToTarget);
                justWalked = true;
            }

            else
            {
                rb.velocity = new Vector2(0, 0) * 0;
                state = NPCState.Idle;
            }

            if(walkingTimer <= 0 && justWalked) //only iterate pattern 1 time when timer is over
            {
                currentPattern = (currentPattern + 1) % movementPattern.Count; //pattern will repeat
                justWalked = false;
            }

            
            walkingTimer -= Time.deltaTime;
        }
    }
}

public enum NPCState { Idle, Walking, Dialog, ClosingDialog }
