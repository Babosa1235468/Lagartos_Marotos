using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;




public class CardInfo
{
    public int life;
    public int damage;
    public int priceSacrifice;
}
class Card
{
    public int life;
    public int damage;
    public int priceSacrifice;
    public List<Effect> allEffects;
    public Card(int cardIndex)
    {
        //Carrega o Json
        TextAsset jsonFile = Resources.Load<TextAsset>("Cards");
        string json = jsonFile.text;

        
        Cards cards = JsonUtility.FromJson<Cards>(json);

        if (cards == null || cardIndex < 0 || cardIndex >= cards.cardInfo.Length)
            throw new Exception("Card inválido ou índice fora do alcance");

        CardInfo card = cards.cardInfo[cardIndex];

        life = card.life;
        damage = card.damage;
        priceSacrifice = card.priceSacrifice;
    }
}

[Serializable]
public class Cards
{
    public CardInfo[] cardInfo;
}
