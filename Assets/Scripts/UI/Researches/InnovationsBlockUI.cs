using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InnovationsBlockUI : BlockUI
{
    private List<ResearchUI> researchUIs;

    public GameObject researchPrefab;
    public RectTransform researchListSpawnPoint;

    public override void Setup(GameData gameData, DepartmentUI parentDepartment)
    {
        base.Setup(gameData, parentDepartment);
    }

    // Start is called before the first frame update
    void Start()
    {
        researchUIs = new List<ResearchUI>();

        //TODO: мне кажется тут нагружается сильно все, но с другой стороны покупка это разовое действие
        foreach(var dep in gameData.Parameters.departments.Values)
        {
            dep.IsBought.SubscribeToModified(x => RebuildContent());

            foreach(var block in dep.blocks.Values)
            {
                block.IsBought.SubscribeToModified(x => RebuildContent());
            }
        }

        RebuildContent();
    }

    public void RebuildContent()
    {
        for(int i = 0; i < researchListSpawnPoint.childCount; i++) 
            Destroy(researchListSpawnPoint.GetChild(i).gameObject);

        var availableResearch = gameData.Researches.Values.Where(c => c.IsAvailable());

        foreach(var res in availableResearch)
        {
            GameObject resUIInstance = Instantiate(researchPrefab, researchListSpawnPoint);

            ResearchUI resUI = resUIInstance.GetComponent<ResearchUI>();

            resUI.Setup(gameData, res);

            researchUIs.Add(resUI);
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
