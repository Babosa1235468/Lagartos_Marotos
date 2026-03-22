using UnityEngine;

public class HatChanger : MonoBehaviour
{
    public void changeHat(int index)
    {
        Sprite hat = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        if(index == 1)
        {
            GameObject.FindGameObjectWithTag("Player1").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = hat;
            DataManager.instance.P1Chapeu = hat; 
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player2").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = hat;
            DataManager.instance.P2Chapeu = hat; 
        }
    }
}
