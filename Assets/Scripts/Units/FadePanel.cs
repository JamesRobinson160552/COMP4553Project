using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadePanel : MonoBehaviour
{

    [SerializeField] private CanvasGroup cg;
    private Tween fadeTween;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitleFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade(float endVal, float duration, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = cg.DOFade(endVal, duration);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        });
    }

    IEnumerator TitleFade()
    {
        yield return new WaitForSeconds(2f);
        FadeOut(2f);
    }
}
