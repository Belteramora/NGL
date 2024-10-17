
using Microsoft.Cci;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

public class DragElement: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rootContainer;
    private RectTransform parentContainer;
    private Vector2 originPosition;
    private Action<DragElement, DragElement> onHoverAction;

    public string appendedBlockName;
    public (int, int) index;
    [HideInInspector]
    public bool isEmpty;


    public RectTransform rectTransform;
    public Image image;

    public Sprite emptySprite;
    public Sprite notEmptySprite;

    public TMP_Text blockLetter;
    public TMP_Text blockText;

    public void Setup(RectTransform rootContainer, RectTransform parentContainer, (int, int) index, Action<DragElement, DragElement> onHoverAction)
    {
        this.appendedBlockName = string.Empty;
        this.rootContainer = rootContainer;
        this.parentContainer = parentContainer;
        this.index = index;
        this.originPosition = rectTransform.anchoredPosition;
        this.onHoverAction = onHoverAction;
    }

    public void SetData(RuntimeBlockData blockData, string blockName)
    {
        image.sprite = notEmptySprite;
        blockLetter.text = blockData.DisplayLetter.Value;
        blockText.text = "block";
        isEmpty = false;
        this.appendedBlockName = blockName;
    }

    public void SetEmpty()
    {
        image.sprite = emptySprite;
        blockLetter.text = string.Empty;
        blockText.text = string.Empty;
        isEmpty = true;
        this.appendedBlockName = string.Empty;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(rootContainer.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, new Vector3(eventData.position.x, eventData.position.y, transform.position.z), 0.5f);
        transform.position = lerpedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var allHovered = GetEventSystemRaycastResults();

        transform.SetParent(parentContainer);
        rectTransform.anchoredPosition = originPosition;

        foreach (var hit in allHovered)
        {
            if (hit.gameObject != gameObject && hit.gameObject.TryGetComponent(out DragElement hoveredElement))
            {
                onHoverAction.Invoke(this, hoveredElement);
                break;
            }
        }

    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
