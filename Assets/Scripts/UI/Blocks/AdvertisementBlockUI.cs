using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementBlockUI : Panel
{
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Hide()
    {
        canvasGroup.Hide();
    }

    public override void Show()
    {
        canvasGroup.Show();
    }
}
