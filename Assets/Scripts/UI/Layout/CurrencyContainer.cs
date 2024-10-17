using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using R3;

public class CurrencyContainer : MonoBehaviour
{
    public TMP_Text currencyName;
    public TMP_Text currencyValue;

    public TMP_Text currencyIncreaseName;
    public TMP_Text currencyIncreaseValue;

    public void Setup(Currency currency)
    {
        currency.CurrencyValue.Subscribe(x => currencyValue.text = x.ToString("G3"));
        currency.CurrencyName.Subscribe(x => currencyName.text = x);
        currency.CurrencyIncreaseValue.SubscribeToModified(x => currencyIncreaseValue.text = x.ToString("G3"));
        currency.CurrencyIncreaseName.Subscribe(x => currencyIncreaseName.text = x);
    }
}
