using BreakInfinity;
using R3;
using UnityEngine;
using UnityEngine.Playables;

public class Currency : IUpdatable
{
    public ReactiveProperty<BigDouble> CurrencyValue { get; set; }
    public ReactiveProperty<string> CurrencyName { get; set; }
    public ReactiveProperty<string> CurrencyShortName { get; set; }
    public GameParameter<BigDouble> CurrencyIncreaseValue { get; set; }
    public ReactiveProperty<string> CurrencyIncreaseName { get; set; }

    public CurrencySetup currencySetupData;

    public Currency(CurrencySetup currencySetupData)
    {
        CurrencyValue = new ReactiveProperty<BigDouble>(10);
        CurrencyName = new ReactiveProperty<string>(currencySetupData.currencyName);
        CurrencyShortName = new ReactiveProperty<string>(currencySetupData.currencyShortName);
        CurrencyIncreaseValue = new GameParameter<BigDouble>(0, false);
        CurrencyIncreaseName = new ReactiveProperty<string>(currencySetupData.currencyShortName + "/s");
        this.currencySetupData = currencySetupData;
    }

    public void Update()
    {
        CurrencyIncreaseValue.Update();
    }
}
