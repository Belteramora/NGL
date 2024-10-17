using System;
using UnityEngine;
using UnityEngine.UI;

public class GridElement: MonoBehaviour
{
    private RuntimeBlockData appendedBlock;
    public DragElement dragElement;
    public RectTransform dragElementContainer;

    public void Setup(RectTransform rootContainer, (int, int) index, Action<DragElement, DragElement> onHoverAction)
    {
        dragElement.Setup(rootContainer, dragElementContainer, index, onHoverAction);
        SetEmpty();
    }

    public void SetData(RuntimeBlockData appendedBlock, string blockName)
    {
        dragElement.SetData(appendedBlock, blockName);
    }

    public void SetEmpty()
    {
        dragElement.SetEmpty();
    }
}
