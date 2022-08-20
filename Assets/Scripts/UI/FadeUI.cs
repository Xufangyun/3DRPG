using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : Singleton<FadeUI>
{
    public float duration;

    private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(this);
    }

    public IEnumerator Fade(int targetAlpha)
    {
        canvasGroup.blocksRaycasts = true;
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / duration);
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
        yield return null;
    }

}
