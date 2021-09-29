using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFX_LadybugMinigame : MonoBehaviour
{
    private SpriteRenderer squareSprite;

    private void Start()
    {
        squareSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        squareSprite.DOFade(1, 0.2f);
    }
    public void FadeOut()
    {
        squareSprite.DOFade(0, 0.5f);
    }
    public void FadeFx()
    {
        Invoke("FadeOut", 0.5f);
        Invoke("FadeIn", 1f);
        Invoke("FadeOut", 1.2f);
        Invoke("FadeIn", 1.7f);
    }
}
