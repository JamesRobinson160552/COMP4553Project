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
    public StaminaBar staminaBar;
    public float currentMoveSpeed;
    public float stamina;
    float reganStaminaCooldown;
    public Unit playerStats;
    bool isResting = false;
    bool isSprinting;
    
    float timeBetweenClicks;
    float timeRemaining_;
    int inputCounter;

    public GameManager gameManager;
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
        stamina = 10;
    }

    private void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        staminaBar.setStamina(stamina);

        if (playerStats.currentHP <= 0)
        {
            StartCoroutine(Respawn());
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.i.leftStartingZone)
        {
            Interact();
            confirmInteractAudio.Play();
        }

        if(Input.GetMouseButton(1) && timeBetweenClicks <= 0 && GameManager.i.leftStartingZone)//(Input.GetKeyDown(KeyCode.R))
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

        if(Input.GetMouseButton(0) && !isLoadingSpell_ && timeRemaining_ <= 0.15 && gameManager.gameActive && (!isSprinting)) //cannot shoot if loading spell or sprinting (or resting)
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
                spellController_.CheckForSpellsIcons();
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

                //Check Sprint //////////////////
                Debug.Log(reganStaminaCooldown);
                if (Input.GetKey(KeyCode.LeftShift) && stamina > 0.0f && (!isResting))
                {
                    Debug.Log("Sprinting???");
                    Sprint();
                    reganStaminaCooldown = 1.0f;
                }
                else if (stamina <= 0 && !isResting) //dont want to reenter this while already resting
                {
                    reganStaminaCooldown -= Time.deltaTime;
                    Debug.Log("rest");
                    //reganStaminaCooldown = 2.0f;
                    StartCoroutine(Rest());
                }
                else if(isResting)
                {
                    currentMoveSpeed = moveSpeed * 0.4f;
                }
                else if (!isResting)
                {
                    reganStaminaCooldown -= Time.deltaTime;
                    Debug.Log("walk");
                    Walk();
                    if(reganStaminaCooldown <= 0)
                        StaminaRegen();
                }
                ////////////////////////////////////

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
        fadeScreen.FadeIn(1f);
        yield return new WaitForSeconds(3f);
        spawnPosition = new Vector3(1.4f, -49.3f, 0);
        if (GameManager.i.reachedBoss)
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
        fadeScreen.FadeOut(1f);
        gameManager.gameActive = true;
    }

    void Sprint()
    {
        isSprinting = true;
        currentMoveSpeed = moveSpeed * 1.5f;
        stamina -= 1.5f * Time.deltaTime;
    }

    void Walk()
    {
        isSprinting = false;
        currentMoveSpeed = moveSpeed;
    }

    void StaminaRegen()
    {
        if (stamina >= 10)
        {
            stamina = 10;
        }
        else
        {
            stamina += 2.0f * Time.deltaTime;
        }
    }

    IEnumerator Rest()
    {
        isResting = true;
        //currentMoveSpeed = moveSpeed * 0.4f;
        yield return new WaitForSeconds(4f);
        isResting = false;
        stamina = 0.1f;
        reganStaminaCooldown = 0f;
        //StaminaRegen();
        //yield return new WaitForSeconds(1f);
    }
}
