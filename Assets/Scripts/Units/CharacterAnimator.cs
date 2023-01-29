using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script to be attached to EVERY UNIT

public class CharacterAnimator : MonoBehaviour
{
    //list of different sprites 
    [SerializeField] List<Sprite> walkLeftArmSprites;
    [SerializeField] List<Sprite> walkRightArmSprites;
    [SerializeField] List<Sprite> attackLeftSprites;
    [SerializeField] List<Sprite> attackRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> castLeftSprites;
    [SerializeField] List<Sprite> castRightSprites;

    public float moveX { get; set; }
    public float moveY { get; set; }
    bool isMoving { get; set; }
    bool isAttacking { get; set; }
    bool isLoadingSpell{ get; set; }

    bool wasPreviouslyMoving_;
    bool isAttackingLeft_;

    int playerInput = 0;

    Vector3 target;

    SpriteAnimator walkLeftArmAnim_;
    SpriteAnimator walkRightArmAnim_;

    SpriteAnimator attackLeft_;
    SpriteAnimator attackRight_;

    SpriteAnimator walkLeftAnim_;
    SpriteAnimator walkRightAnim_;

    SpriteAnimator castLeftAnim_;
    SpriteAnimator castRightAnim_;

    SpriteAnimator currentAnimBody_; //selected animation
    SpriteAnimator currentAnimArm_;

    //SpriteRenderer spriteRenderer_;
    //SpriteRenderer spriteRendererArm_;

    List<SpriteRenderer> SpriteRenderers_ = new List<SpriteRenderer>();

    private void Start()
    {
        GetComponentsInChildren(SpriteRenderers_);

        if(SpriteRenderers_.Count > 1)
        {
            walkLeftArmAnim_ = new SpriteAnimator(walkLeftArmSprites, SpriteRenderers_[1]);
            walkRightArmAnim_ = new SpriteAnimator(walkRightArmSprites, SpriteRenderers_[1]);

            castLeftAnim_ = new SpriteAnimator(castLeftSprites, SpriteRenderers_[1]);
            castRightAnim_ = new SpriteAnimator(castRightSprites, SpriteRenderers_[1]);
        
            attackLeft_ = new SpriteAnimator(attackLeftSprites, SpriteRenderers_[1]);
            attackRight_ = new SpriteAnimator(attackRightSprites, SpriteRenderers_[1]);
        }

        else
        {
            walkLeftArmAnim_ = new SpriteAnimator(walkLeftSprites, SpriteRenderers_[0]);
            walkRightArmAnim_ = new SpriteAnimator(walkRightSprites, SpriteRenderers_[0]);

            castLeftAnim_ = new SpriteAnimator(walkLeftSprites, SpriteRenderers_[0]);
            castRightAnim_ = new SpriteAnimator(walkRightSprites, SpriteRenderers_[0]);
        
            attackLeft_ = new SpriteAnimator(walkLeftSprites, SpriteRenderers_[0]);
            attackRight_ = new SpriteAnimator(walkRightSprites, SpriteRenderers_[0]);
        }

        walkLeftAnim_ = new SpriteAnimator(walkLeftSprites, SpriteRenderers_[0]);
        walkRightAnim_ = new SpriteAnimator(walkRightSprites, SpriteRenderers_[0]);

        currentAnimBody_ = walkRightAnim_;
        currentAnimArm_ = walkRightArmAnim_;

        isAttacking = false;
        isMoving = false;
    }

    private void Update()
    {
        var prevAnim_ = currentAnimBody_;

        //set current animation depending on which direction is input
        if(isAttacking == false)
        {
            if (moveX == 1 || (moveY == 1 && moveX == 0))
            {
                currentAnimBody_ = walkRightAnim_;
                currentAnimArm_ = walkRightArmAnim_;
            }

            else
            {
                currentAnimBody_ = walkLeftAnim_;
                currentAnimArm_ = walkLeftArmAnim_;
            }
        }
        //have character face in direction of attack
        else
        {
            if (isAttackingLeft_ == false)
            {
                currentAnimBody_ = walkRightAnim_;
                currentAnimArm_ = attackRight_;
            }

            else
            {
                currentAnimBody_ = walkLeftAnim_;
                currentAnimArm_ = attackLeft_;
            }
        }

        if(isLoadingSpell)
        {
            if(currentAnimBody_ == walkRightAnim_)
            {    
                currentAnimArm_ = castRightAnim_;
                SpriteRenderers_[1].sprite = currentAnimArm_.Frames[playerInput];
            }

            else
            {
                currentAnimArm_ = castLeftAnim_;
                SpriteRenderers_[1].sprite = currentAnimArm_.Frames[playerInput];
            }
        }

        //if running against wall no animations will be played
        if (currentAnimBody_ != prevAnim_ || isMoving != wasPreviouslyMoving_)
        {
            currentAnimBody_.Start();
            currentAnimArm_.Start();
        }

        //plays loop, first 2 lines keep animations synced
        if (isMoving)
        {
            currentAnimArm_.currentFrame = currentAnimBody_.currentFrame;
            currentAnimArm_.timer = currentAnimBody_.timer;
            currentAnimBody_.HandleUpdate();
            currentAnimArm_.HandleUpdate();

        }

        else //resets animation loop when current loop is stopped
        {
            SpriteRenderers_[0].sprite = currentAnimBody_.Frames[0];
            if(SpriteRenderers_.Count > 1 && !isLoadingSpell)
                SpriteRenderers_[1].sprite = currentAnimArm_.Frames[0];
        }

        wasPreviouslyMoving_ = isMoving;
    }

    public void ChangeIsMoving(bool state)
    {
        isMoving = state;
    }

    public void ChangeIsLoadingSpell(bool state)
    {
        isLoadingSpell = state;
    }

    public void ChangePlayerInputs(int value)
    {
        playerInput = value;
    }

    public void ChangeIsAttacking(bool state)
    {
        isAttacking = state;
    }

    public void AttackPos(Vector3 targetSpot)
    {
        isAttacking = true;

        if(transform.position.x > targetSpot.x)
        {
            isAttackingLeft_ = true;
        }
        else
        {
            isAttackingLeft_ = false;
        }
    }
    

}


