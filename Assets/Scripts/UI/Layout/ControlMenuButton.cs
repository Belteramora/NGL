using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlMenuButton : MonoBehaviour
{
    private RuntimeDepartmentData appendedDepartment;
    private string departmentKey;

    private Action<string> onBuyAction;
    private Action<string> onTransitionAction;

    public TMP_Text buyButtonDepartmentName;
    public TMP_Text transitionButtonDepartmentName;
    public TMP_Text costTextLabel;


    public GameObject buyButton;
    public GameObject transitionButton;

    //TODO: поправить, криво сделано штука с подменой gameObject'а при покупке департамента
    public void Setup(GameData gameData, RuntimeDepartmentData department, string departmentKey, Action<string> onBuyAction, Action<string> onTransitionAction)
    {
        this.appendedDepartment = department;
        this.departmentKey = departmentKey;

        this.onBuyAction = onBuyAction;
        this.onTransitionAction = onTransitionAction;

        department.DisplayName.SubscribeToModified(x =>
        {
            buyButtonDepartmentName.text = x;
            transitionButtonDepartmentName.text = x;
        });

        department.IsBought.SubscribeToBase(OnChangeBought);

        department.Cost.SubscribeToModified(x => costTextLabel.text = "Cost: " + x.ToString("G3") + " " + gameData.Currencies["main"].CurrencyShortName);

    }

    public void OnBuy()
    {
        onBuyAction?.Invoke(departmentKey);
    }

    public void OnTransition()
    {
        onTransitionAction?.Invoke(departmentKey);
    }

    public void OnChangeBought(bool isBoight)
    {
        buyButton.SetActive(!isBoight);
        transitionButton.SetActive(isBoight);
    }
}
