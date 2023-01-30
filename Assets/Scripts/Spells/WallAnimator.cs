using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> activeFrames;
    [SerializeField] List<Sprite> damagedFrames;

    public float breakingPoint;
    float lifeRemaining = 0;
    SpriteAnimator activeAnim_;
    SpriteAnimator damagedAnim_;

    SpriteRenderer renderer_;

    private void Start()
    {
        renderer_ = gameObject.GetComponent<SpriteRenderer>();

        activeAnim_ = new SpriteAnimator(activeFrames, renderer_);
        damagedAnim_ = new SpriteAnimator(damagedFrames, renderer_);

        activeAnim_.Start();
    }

    private void Update()
    {
        lifeRemaining = gameObject.GetComponent<DestroyMe>().lifeSpan;

        if(lifeRemaining >= breakingPoint)
            activeAnim_.HandleUpdate();
        else
            damagedAnim_.HandleUpdate();
    }
    
}
