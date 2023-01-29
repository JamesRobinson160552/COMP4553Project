using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> activeFrames;

    SpriteAnimator activeAnim_;

    SpriteRenderer renderer_;

    private void Start()
    {
        renderer_ = gameObject.GetComponent<SpriteRenderer>();

        activeAnim_ = new SpriteAnimator(activeFrames, renderer_);

        activeAnim_.Start();
    }

    private void Update()
    {
        activeAnim_.HandleUpdate();
    }
    
}
