using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;  // Links rigidbody to script to allow for movement
    Vector2 movement;  // Stores x and y 
    public GameManager gameManager;

    Character character;
    float timeRemaining_;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButton(0))
        {
            character.Animator.AttackPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            timeRemaining_ = 0.5f;
        }
    }

    // Use FixedUpdate as it is executed on a fixed timer (default = 50times/second)
    void FixedUpdate()
    {
        // Movement
        if (gameManager.gameActive) //Only allow movement when game is active
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            
            if(timeRemaining_ > 0)
            {
                timeRemaining_ -= Time.fixedDeltaTime;
            }
            else
            {
                character.Animator.ChangeIsAttacking(false);
            }
        }
        
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
}
