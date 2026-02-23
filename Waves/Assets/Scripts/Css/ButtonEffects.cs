using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffectsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject whiteSquare;

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (whiteSquare == null) return;

        RectTransform buttonRect = GetComponent<RectTransform>();
        RectTransform squareRect = whiteSquare.GetComponent<RectTransform>();

        if (buttonRect != null && squareRect != null)
        {
            // Width Fixed (1920x1080) and the height if the button
            Vector2 size = squareRect.sizeDelta;
            size.y = buttonRect.sizeDelta.y;
            squareRect.sizeDelta = size;

            Vector3 pos = transform.position;
            pos.x = 960;
            whiteSquare.transform.position = pos;
        }

        whiteSquare.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (whiteSquare == null) return;
        whiteSquare.SetActive(false);
    }
}
