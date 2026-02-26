using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffectsUI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public GameObject whiteSquare;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (whiteSquare == null) return;

        RectTransform buttonRect = GetComponent<RectTransform>();
        RectTransform squareRect = whiteSquare.GetComponent<RectTransform>();

        if (buttonRect != null && squareRect != null)
        {
            // Match size
            squareRect.sizeDelta = buttonRect.sizeDelta;

            // Center it perfectly
            squareRect.anchorMin = new Vector2(0.5f, 0.5f);
            squareRect.anchorMax = new Vector2(0.5f, 0.5f);
            squareRect.pivot = new Vector2(0.5f, 0.5f);
            squareRect.anchoredPosition = Vector2.zero;
        }

        whiteSquare.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (whiteSquare == null) return;
        whiteSquare.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (whiteSquare == null) return;
        whiteSquare.SetActive(false);
    }
    
}