using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;  // Links rigidbody to script to allow for movement
    Vector2 movement;  // Stores x and y 
    bool isLoadingSpell_;
    public GameManager gameManager;
    public Unit playerStats;

    SpellController spellController_;
    Character character;
    float timeBetweenClicks;
    float timeRemaining_;
    int inputCounter;

    private void Awake()
    {
        character = GetComponent<Character>();
        spellController_ = GetComponent<SpellController>();
        isLoadingSpell_ = false;
    }

    private void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButton(1) && timeBetweenClicks <= 0)//(Input.GetKeyDown(KeyCode.R))
        {
            timeBetweenClicks = 0.25f;
            isLoadingSpell_ = !(isLoadingSpell_);
            character.Animator.ChangeIsLoadingSpell(isLoadingSpell_);
            if(isLoadingSpell_)
                {
                    spellController_.resetPlayerInputs();
                    inputCounter = 0;
                    character.Animator.ChangePlayerInputs(0);
                }
        }

        if(Input.GetMouseButton(0) && !isLoadingSpell_ && timeRemaining_ <= 0.25 && gameManager.gameActive) //cannot shoot if loading spell
        {
            character.Animator.AttackPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            spellController_.CheckForSpells();
            timeRemaining_ = 0.5f;
        }

        if(isLoadingSpell_)
        {
            if(Input.GetKeyDown(KeyCode.A))
                {
                    character.Animator.ChangePlayerInputs(1);
                    spellController_.AddToSpellCommand('A');
                    inputCounter++;
                }

            if(Input.GetKeyDown(KeyCode.W))
                {
                    character.Animator.ChangePlayerInputs(2);
                    spellController_.AddToSpellCommand('W');
                    inputCounter++;
                }

            if(Input.GetKeyDown(KeyCode.S))
                {
                    character.Animator.ChangePlayerInputs(3);
                    spellController_.AddToSpellCommand('S');
                    inputCounter++;
                }

            if(Input.GetKeyDown(KeyCode.D))
                {
                    character.Animator.ChangePlayerInputs(4);
                    spellController_.AddToSpellCommand('D');
                    inputCounter++;
                }
            if(inputCounter == 4)
            {
                isLoadingSpell_ = false;
                character.Animator.ChangeIsLoadingSpell(isLoadingSpell_);
                inputCounter = 0;
                character.Animator.ChangePlayerInputs(0);
            }
        }
    }

    // Use FixedUpdate as it is executed on a fixed timer (default = 50times/second)
    void FixedUpdate()
    {
        // Movement
        if (gameManager.gameActive) //Only allow movement when game is active
        {
            if(isLoadingSpell_ == false) //cannot walk if loading spell
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
                //tells character script if unit is moving or not
                if(movement != Vector2.zero)
                {
                    character.Moving(movement);
                }
                else
                {
                    character.Animator.ChangeIsMoving(false);
                }
            }

            if(isLoadingSpell_ == true)
            {
                character.Animator.ChangeIsMoving(false);
            }
            
            if(timeRemaining_ > 0)// && !isLoadingSpell_)
            {
                timeRemaining_ -= Time.fixedDeltaTime;
            }
            else
            {
                character.Animator.ChangeIsAttacking(false);
            }

            if(timeBetweenClicks > 0)
            {
                timeBetweenClicks -= Time.fixedDeltaTime;
            }
        }
        
    }
}
