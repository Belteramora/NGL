
using TMPro;
using UnityEngine;

public class BlockUI : Panel
{
    protected GameData gameData;
    protected CanvasGroup canvasGroup;

    protected RuntimeBlockData block;

    public TMP_Text displayNameLabel;

    public virtual void Setup(GameData gameData, DepartmentUI parentDepartment)
    {
        this.gameData = gameData;
        canvasGroup = GetComponent<CanvasGroup>();

        block = gameData.Parameters[parentDepartment.panelName][panelName];

        block.DisplayName.SubscribeToModified(x => displayNameLabel.text = x);
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
