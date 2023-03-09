using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;  // Links rigidbody to script to allow for movement
    Vector2 movement;  // Stores x and y
    bool isLoadingSpell_;
    SpellController spellController_;
    Character character;
    public HealthBar healthBar;
    public float currentMoveSpeed;
    public Unit playerStats;
    
    float timeBetweenClicks;
    float timeRemaining_;
    int inputCounter;

    public GameManager gameManager;
    public bool reachedBoss = false;
    public Vector3 spawnPosition;
    public FadePanel fadeScreen;
    public GameObject crow;

    public AudioSource confirmInteractAudio;
    public AudioSource deathSound;

    private void Awake()
    {
        currentMoveSpeed = moveSpeed;
        character = GetComponent<Character>();
        spellController_ = GetComponent<SpellController>();
        playerStats = GetComponent<Unit>();
        isLoadingSpell_ = false;
    }

    private void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (playerStats.currentHP <= 0)
        {
            StartCoroutine(Respawn());
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.i.leftStartingZone)
        {
            Interact();
            confirmInteractAudio.Play();
        }

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
        if (gameManager.gameActive && gameManager.showingDialog == false) //Only allow movement when game is active
        {
            if(isLoadingSpell_ == false) //cannot walk if loading spell
            {
                if(movement.x == 1 && movement.y == 1) 
                    movement = new Vector2(0.70f, 0.70f);

                if(movement.x == -1 && movement.y == -1) 
                    movement = new Vector2(-0.70f, -0.70f);

                if(movement.x == -1 && movement.y == 1) 
                    movement = new Vector2(-0.70f, 0.70f);

                if(movement.x == 1 && movement.y == -1) 
                    movement = new Vector2(0.70f, -0.70f);

                rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);
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

        else
        {
            rb.MovePosition(rb.position + movement * 0 * Time.fixedDeltaTime);
            character.Animator.ChangeIsMoving(false);
        }
    }

    void Interact()
    {
        if(GameManager.i.gameActive)
        {
            var facingDir = new Vector3(character.Animator.moveX, character.Animator.moveY);
            var interactPos = transform.position + facingDir; //the adjacent tile the playing is facing

            var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
            if(collider != null)
            {
                //Debug.Log("talking");
                //collider.GetComponent<NPCController>(); //looks for this script in the object trying to be interacted with
                collider.GetComponent<Interactible>()?.Interact(transform);
            }

            else
            {
                Crow crow = GameObject.Find("Crow").GetComponent<Crow>();
                StartCoroutine(crow.TalkToCrow());
            }
        }
    }

    //Resets enemies and sends player to spawn location with full health
    IEnumerator Respawn()
    {
        deathSound.Play();
        gameManager.gameActive = false;
        fadeScreen.FadeIn(2f);
        yield return new WaitForSeconds(3f);
        spawnPosition = new Vector3(1.4f, -49.3f, 0);
        if (reachedBoss)
        {
            gameManager.EndBossMusic();
            spawnPosition = new Vector3(-60.0f, 12.0f, 0);
        }
        playerStats.currentHP = playerStats.GetUnitBase.MaxHP;
        healthBar.setHealth(playerStats.currentHP);
        gameManager.ResetEnemies();
        crow.transform.position = spawnPosition;
        transform.position = spawnPosition;
        yield return new WaitForSeconds(2f);
        fadeScreen.FadeOut(2f);
        gameManager.gameActive = true;
    }
}
