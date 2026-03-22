using UnityEngine;

public class MouthChanger : MonoBehaviour
{
    public void changeMouth(int index)
    {
        Sprite mouth = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        if(index == 1)
        {
            GameObject.FindGameObjectWithTag("Player1").transform.Find("Sprites/Mouth").GetComponent<SpriteRenderer>().sprite = mouth;
            DataManager.instance.P1Mouth = mouth; 
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player2").transform.Find("Sprites/Mouth").GetComponent<SpriteRenderer>().sprite = mouth;
            DataManager.instance.P2Mouth = mouth; 
        }
    }
}
