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

        //atribuir as cores
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag != "DoNotChange")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = clickedColor;
            }  
        }
    }
}