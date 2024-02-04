using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup canvasGroup;
    [SerializeField]
    public CanvasGroup fadeOutCanvasGroup;

    [SerializeField]
    public float StepsFadeIn = 0.1f;
    [SerializeField]
    public float StepsFadeOut = 0.1f;


    public bool fadein = false;
    public bool fadeout = false;

    private float _fps = 72f;
    // Update is called once per frame
    void Update()
    {
        if (fadein)
        {
            //Debug.Log("  => Fade in");
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += StepsFadeIn * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    fadein = false;
                }
            }
        }
        if (fadeout)
        {
            //Debug.Log("  => Fade out");
            if (fadeOutCanvasGroup.alpha >= 0)
            {
                fadeOutCanvasGroup.alpha -= StepsFadeOut * Time.deltaTime;
                if (fadeOutCanvasGroup.alpha == 0)
                {
                    fadeout = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        Debug.Log("Fade in");
        fadein = true;
    }

    public void FadeOut()
    {
        fadeout = true;
    }
}
