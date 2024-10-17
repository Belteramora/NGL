using R3;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchUI: MonoBehaviour
{
    private ResearchBase appendedResearch;
    private GameData gameData;
    private bool isBought = false;

    private List<IDisposable> disposables = new List<IDisposable>();

    private IDisposable baseCostDisposable;

    public TMP_Text displayNameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public TMP_Text effectText;
    public Button button;

    public void Setup(GameData gameData, ResearchBase appendedResearch)
    {
        this.appendedResearch = appendedResearch;
        this.gameData = gameData;


        displayNameText.text = appendedResearch.displayName;
        descriptionText.text = appendedResearch.description;
        appendedResearch.Effect.SubscribeToModified((x) =>
        {
            effectText.text = "Effect: " + appendedResearch.effectSign + x.ToString("G3");
        }).AddTo(disposables);

        SetButtonToDefault();

        appendedResearch.IsBought.SubscribeToModified(bought =>
        {
            //TODO: тут также oneShot штука
            if(!isBought && bought)
            {
                isBought = true;
                OnPurchase();
            }
            else if(isBought && !bought)
            {
                isBought = false;
                SetButtonToDefault();
            }
        }).AddTo(disposables);

        
    }

    //TODO: поправить механизм покупки/отмены покупки

    public void OnPurchase()
    {
        baseCostDisposable?.Dispose();

        costText.text = "Bought";

        button.interactable = false;

    }

    public void SetButtonToDefault()
    {
        button.interactable = true;

        baseCostDisposable = appendedResearch.BaseCost.SubscribeToModified((x) =>
        {
            costText.text = "Cost: " + x.ToString("G3");
        });
        
    }

    public bool CanBuy() 
    {
        return gameData.Currencies["main"].CurrencyValue.Value >= appendedResearch.BaseCost.Value; 
    }


    public void OnBuy()
    {
        if (CanBuy())
        {
            gameData.Currencies["main"].CurrencyValue.Value -= appendedResearch.BaseCost.Value;

            appendedResearch.IsBought.BaseValue = true;   
        }
    }

    private void OnDestroy()
    {
        baseCostDisposable?.Dispose();

        foreach(var disposable in disposables) 
        {
            disposable.Dispose();
        }
    }
}