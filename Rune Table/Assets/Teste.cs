using Unity.VisualScripting;
using UnityEngine;

public class Teste : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Clicaste");
        Card card = new Card(1);
        Debug.Log(card.damage);
        Debug.Log(card.life);
        Debug.Log(card.priceSacrifice);
    }
    private void OnMouseDown()
    {
        
    }
}
