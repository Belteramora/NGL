using DG.Tweening;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class UI : MonoBehaviour
{
    public DepartmentUI currentDepartment;
    public string defaultDepartmentName;
    public List<DepartmentUI> departments;

    public CanvasGroup screensContainer;
    public ControlMenu controlMenu;
    public NavigationBar navigationBar;

    public GameObject currencyContainerPrefab;
    public RectTransform currencyContainersSpawnPoint;

    private List<CurrencyContainer> currencyContainers;

    public Action<DepartmentUI> onChangeDepartment;

    [Inject]
    public void Construct(GameData gameData)
    {
        currencyContainers = new List<CurrencyContainer>();

        //TODO: очень запутанная херня, стоит поменять
        

        foreach(var currency in gameData.Currencies.Values)
        {
            var currencyContainerInstance = Instantiate(currencyContainerPrefab, currencyContainersSpawnPoint);
            var currencyContainer = currencyContainerInstance.GetComponent<CurrencyContainer>();
            currencyContainer.Setup(currency);
            currencyContainers.Add(currencyContainer);
        }


        controlMenu.Setup(gameData, ChangeDepartment);
        navigationBar.Setup(gameData, ref onChangeDepartment);

        ChangeDepartment(defaultDepartmentName);
    }

    public void OpenMenu()
    {
        screensContainer.Hide();

        controlMenu.Show();

    }

    public void ChangeDepartment(string departmentName)
    {
        //if (panelName == currentPanel.panelName) return;

        screensContainer.Show();
        controlMenu.Hide();

        currentDepartment?.Hide();

        currentDepartment = departments.First(c => c.panelName == departmentName);
        currentDepartment.Show();

        onChangeDepartment?.Invoke(currentDepartment);
    }
}