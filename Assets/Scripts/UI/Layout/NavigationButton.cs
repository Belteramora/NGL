using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public List<IDisposable> disposables = new List<IDisposable>();

    private string appendedBlockName;

    public GameObject unlockFrame;
    public GameObject toBlockFrame;


    public TMP_Text letter;
    public TMP_Text costLabel;

    public void Setup(string appendedBlockName, RuntimeBlockData blockData, Action<string> onBuy, Action<string> onChangeBlock)
    {
        this.appendedBlockName = appendedBlockName;

        blockData.Cost.SubscribeToModified(x => costLabel.text = "Cost: " + x.ToString("G3")).AddTo(disposables);
        blockData.DisplayLetter.SubscribeToModified(x => letter.text = x).AddTo(disposables);

        blockData.IsBought.SubscribeToBase(isBought =>
        {
            var needUnlock = !isBought;
            if (blockData.Cost.Value == 0)
                needUnlock = false;

            unlockFrame.SetActive(needUnlock);
            toBlockFrame.SetActive(!needUnlock);
        }).AddTo(disposables);

        unlockFrame.GetComponent<Button>().onClick.AddListener(() => onBuy.Invoke(appendedBlockName));
        toBlockFrame.GetComponent<Button>().onClick.AddListener(() => onChangeBlock.Invoke(appendedBlockName));
    }

    private void OnDestroy()
    {
        foreach(var disposable in disposables)
        {
            disposable.Dispose();
        }
    }
}
