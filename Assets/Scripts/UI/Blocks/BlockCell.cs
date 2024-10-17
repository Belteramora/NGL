using BreakInfinity;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockCell : MonoBehaviour
{
    private IDisposable disposable;
    private GameParameter<BigDouble> cellBaseValue;
    private bool isBought;


    public TMP_Text buttonText;
    public Image cellImage;
    public Sprite cellNotBought;
    public Sprite cellBought;

    public void Setup(int index, GameParameter<long> cellsBought, GameParameter<BigDouble> cellBaseValue)
    {
        this.cellBaseValue = cellBaseValue;
        cellsBought.SubscribeToModified((cb) =>
        {
            var bought = index < cb;

            //TODO: По факту oneShot эффект, срабатывает разово
            if (!isBought && bought)
            {
                isBought = true;

                OnPurchase();
            }
            else if(isBought && !bought)
            {
                isBought = false;
                CancelPurchase();
            }
        });
    }

    private void OnPurchase()
    {
        cellImage.sprite = cellBought;
        disposable = cellBaseValue.SubscribeToModified((x) => buttonText.text = x.ToString("G3"));
    }

    private void CancelPurchase()
    {
        disposable?.Dispose();

        buttonText.text = "";

        cellImage.sprite = cellNotBought;
    }
}
