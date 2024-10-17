using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class NavigationBar : MonoBehaviour
{
    private GameData gameData;
    private DepartmentUI currentDepartment;

    public GameObject navigationButtonPrefab;
    public RectTransform buttonsContainer;



    public void Setup(GameData gameData, ref Action<DepartmentUI> onChangeDepartment)
    {
        Debug.Log("CONSTRUCT NAV BAR");
        this.gameData = gameData;

        onChangeDepartment += OnChangeDepartment;
    }

    public void OnChangeDepartment(DepartmentUI department)
    {
        this.currentDepartment = department;
        for(int i = 0; i < buttonsContainer.childCount; i++)
        {
            Destroy(buttonsContainer.GetChild(i).gameObject);
        }

        foreach (var block in gameData.Parameters[department.panelName].blocks)
        {
            var buttonInstance = Instantiate(navigationButtonPrefab, buttonsContainer);

            NavigationButton button = buttonInstance.GetComponent<NavigationButton>();

            button.Setup(block.Key, block.Value, BuyBlock, ChangeBlock);
        }
    }

    public void BuyBlock(string blockName)
    {
        if (gameData.Parameters[currentDepartment.panelName][blockName].Cost.Value > gameData.Currencies["main"].CurrencyValue.Value) return;

        gameData.Currencies["main"].CurrencyValue.Value -= gameData.Parameters[currentDepartment.panelName][blockName].Cost.Value;

        gameData.Parameters[currentDepartment.panelName][blockName].IsBought.BaseValue = true;

    }

    public void ChangeBlock(string blockName)
    {
        currentDepartment.ChangeBlock(blockName);
    }
}
