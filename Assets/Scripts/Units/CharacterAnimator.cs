using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script to be attached to EVERY UNIT

public class CharacterAnimator : MonoBehaviour
{
    //list of different sprites 
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    public float moveX { get; set; }
    public float moveY { get; set; }
    public bool isMoving { get; set; }

    bool wasPreviouslyMoving_;

    SpriteAnimator walkDownAnim_;
    SpriteAnimator walkUpAnim_;
    SpriteAnimator walkLeftAnim_;
    SpriteAnimator walkRightAnim_;

    SpriteAnimator currentAnim_; //selected animation

    SpriteRenderer spriteRenderer_;

    private void Start()
    {
        //setting different animation loops depending on actions
        spriteRenderer_ = GetComponent<SpriteRenderer>();
        walkDownAnim_ = new SpriteAnimator(walkDownSprites, spriteRenderer_);
        walkUpAnim_ = new SpriteAnimator(walkUpSprites, spriteRenderer_);
        walkLeftAnim_ = new SpriteAnimator(walkLeftSprites, spriteRenderer_);
        walkRightAnim_ = new SpriteAnimator(walkRightSprites, spriteRenderer_);

        currentAnim_ = walkDownAnim_;
    }

    private void Update()
    {
        var prevAnim_ = currentAnim_;

        //set current animation depending on which direction is input
        if (moveX == 1)
        {
            currentAnim_ = walkRightAnim_;
        }

        else if (moveX == -1)
        {
            currentAnim_ = walkLeftAnim_;
        }

        else if (moveY == 1)
        {
            currentAnim_ = walkUpAnim_;
        }

        else if (moveY == -1)
        {
            currentAnim_ = walkDownAnim_;
        }

        //if running against wall no animations will be played
        if (currentAnim_ != prevAnim_ || isMoving != wasPreviouslyMoving_)
            currentAnim_.Start();

        //plays loop
        if (isMoving)
            currentAnim_.HandleUpdate();

        else //resets animation loop when current loop is stopped
            spriteRenderer_.sprite = currentAnim_.Frames[0];

        wasPreviouslyMoving_ = isMoving;
    }

}


