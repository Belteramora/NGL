using BreakInfinity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ControlMenu : Panel
{
    private GameData gameData;
    private CanvasGroup canvasGroup;

    public GameObject menuButtonPrefab;
    public RectTransform menuButtonsContainer;


    public void Setup(GameData gameData, Action<string> changeDepartmentAction)
    {
        this.gameData = gameData;

        canvasGroup = GetComponent<CanvasGroup>();

        var departments = gameData.Parameters.departments;

        foreach(var dep in departments)
        {
            var menuButtonInstance = Instantiate(menuButtonPrefab, menuButtonsContainer);

            var menuButton = menuButtonInstance.GetComponent<ControlMenuButton>();

            menuButton.Setup(gameData, dep.Value, dep.Key, OnBuyDepartment, changeDepartmentAction);
        }
    }

    public void OnBuyDepartment(string departmentKey)
    {
        Debug.Log("on buy dep " + departmentKey);

        if (gameData.Currencies["main"].CurrencyValue.Value >= gameData.Parameters[departmentKey].Cost.Value)
        {
            gameData.Currencies["main"].CurrencyValue.Value -= gameData.Parameters[departmentKey].Cost.Value;
            gameData.Parameters[departmentKey].IsBought.BaseValue = true;

        }
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
