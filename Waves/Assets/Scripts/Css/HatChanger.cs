using UnityEngine;

public class HatChanger : MonoBehaviour
{
    public void changeHat(int index)
    {
        Sprite hat = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

        if (index == 1)
        {
            if (DataManager.instance.P1Chapeu == hat)
            {
                GameObject.FindGameObjectWithTag("Player1").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = null;
                DataManager.instance.P1Chapeu = null;
                return;
            }
            GameObject.FindGameObjectWithTag("Player1").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = hat;
            DataManager.instance.P1Chapeu = hat;
        }
        else
        {
            if (DataManager.instance.P2Chapeu == hat)
            {
                GameObject.FindGameObjectWithTag("Player2").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = null;
                DataManager.instance.P2Chapeu = null;
                return;
            }
            GameObject.FindGameObjectWithTag("Player2").transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = hat;
            DataManager.instance.P2Chapeu = hat;
        }
    }
}
