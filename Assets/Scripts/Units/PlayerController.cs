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

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    // Use FixedUpdate as it is executed on a fixed timer (default = 50times/second)
    void FixedUpdate()
    {
        // Movement
        if (gameManager.gameActive) //Only allow movement when game is active
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
        //tells character script if unit is moving or not
        if(movement != Vector2.zero)
        {
            character.Moving(movement);
        }

        else
        {
            character.StoppedMoving();
        }
    }
}
