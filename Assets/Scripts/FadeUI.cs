using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField]
    float fadeSpeed = 0.03f;
    enum fadeModes { none, fadeIn, fadeOut };
    byte fadeMode;

    CanvasGroup canvas;

    void Awake()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (!cg)
            cg = gameObject.AddComponent<CanvasGroup>();
        canvas = cg;
    }

    void FixedUpdate()
    {
        if (fadeMode != (byte)fadeModes.none)
        {
            canvas.alpha = fadeMode == (byte)fadeModes.fadeIn ? Mathf.Min(canvas.alpha + fadeSpeed, 1f) : Mathf.Max(canvas.alpha - fadeSpeed, 0f);

            //stop fading when at least one child is back to normal
            if (canvas.alpha == (fadeMode == (byte)fadeModes.fadeIn ? 1f : 0f))
                fadeMode = (byte)fadeModes.none;
        }
    }

    public void FadeIn()
    {
        Fade((byte)fadeModes.fadeIn);
    }

    public void FadeOut()
    {
        Fade((byte)fadeModes.fadeOut);
    }

    void Fade(byte fadeMode)
    {
        GetComponent<Canvas>().enabled = true;

        canvas.alpha = fadeMode == (byte)fadeModes.fadeIn ? 0f : 1f;
        this.fadeMode = fadeMode;
    }
}
