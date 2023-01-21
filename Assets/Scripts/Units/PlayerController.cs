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

    SpellController spellController_;
    Character character;
    float timeRemaining_;

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

        if(Input.GetKeyDown(KeyCode.R))
        {
            isLoadingSpell_ = !(isLoadingSpell_);
            if(isLoadingSpell_)
                spellController_.resetPlayerInputs();
        }

        if(Input.GetMouseButton(0) && !isLoadingSpell_ && timeRemaining_ <= 0.25 && gameManager.gameActive) //cannot shoot if loading spell
        {
            character.Animator.AttackPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            spellController_.CheckForSpells();
            timeRemaining_ = 0.5f;
        }

        if(isLoadingSpell_)
        {
            if(Input.GetKeyDown(KeyCode.W))
                spellController_.AddToSpellCommand('W');

            if(Input.GetKeyDown(KeyCode.A))
                spellController_.AddToSpellCommand('A');

            if(Input.GetKeyDown(KeyCode.S))
                spellController_.AddToSpellCommand('S');

            if(Input.GetKeyDown(KeyCode.D))
                spellController_.AddToSpellCommand('D');
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
        }
        
    }
}
