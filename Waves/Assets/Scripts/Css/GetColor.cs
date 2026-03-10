using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GetColor : MonoBehaviour
{
    public void ChangeColorP1(GameObject obj)
    {
        ChangeColor(obj, 1);
    }

    public void ChangeColorP2(GameObject obj)
    {
        ChangeColor(obj, 2);
    }

    private void ChangeColor(GameObject obj, int player)
    {
        Image img = obj.GetComponent<Image>();
        Color clickedColor = img.color;
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("DoNotChange"))
            {
                if (child.TryGetComponent(out SpriteRenderer sr))
                    sr.color = clickedColor;
            }
        }

        if (player == 1)
        {
            DataManager.instance.P1SpriteColor = clickedColor;
        }
        else
        {
            DataManager.instance.P2SpriteColor = clickedColor;
        }
    }
}