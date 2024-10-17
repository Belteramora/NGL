using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class DepartmentUI : Panel
{

    public CanvasGroup canvasGroup;
    public TMP_Text displayNameLabel;
    public BlockUI currentPanel;
    public string defaultBlockName;
    public List<BlockUI> blocks;

    [Inject]
    public void Construct(GameData gameData)
    {

        Debug.Log("Dep ui start construct");

        gameData.Parameters[panelName].DisplayName.SubscribeToModified(x => displayNameLabel.text = x);

        foreach (var block in blocks)
        {
            block.Setup(gameData, this);
        }

        currentPanel = blocks.First(c => c.panelName == defaultBlockName);
        currentPanel.Show();
    }

    public void ChangeBlock(string blockName)
    {
        if (blockName == currentPanel.panelName) return;


        currentPanel.Hide();

        currentPanel = blocks.First(c => c.panelName == blockName);
        currentPanel.Show();
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
