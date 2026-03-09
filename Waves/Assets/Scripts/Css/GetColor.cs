using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

using Microsoft.Unity.VisualStudio.Editor;
public class GetColor : MonoBehaviour
{
    public void ChangeColor(GameObject obj)
    {
        Image img = obj.GetComponent<Image>();

        //clikec collor e a cor com qual clicou
        Color clickedColor = img.color;

        gameObject.GetComponent<Image>().color = clickedColor;
    }
}