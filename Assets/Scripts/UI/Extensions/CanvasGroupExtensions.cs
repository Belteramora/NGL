
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public static class CanvasGroupExtensions
{
    public static void Hide(this CanvasGroup canvasGroup, float animationTime = 0.2f)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, animationTime);
    }

    public static void Show(this CanvasGroup canvasGroup, float animationTime = 0.2f)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, animationTime);
    }
}

