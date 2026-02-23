using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[Serializable]
public class CardInfo
{
    public int Index;
    public int life;
    public int damage;
    public int priceSacrifice;
}

[Serializable]
public class Cards
{
    public CardInfo[] cardInfo;
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
        foreach (CardInfo card in cards.cardInfo)
        {
            if (cardIndex == card.Index)
            {
                this.life = card.life;
                this.damage = card.damage;
                this.priceSacrifice = card.priceSacrifice;
                return;
            }
        }
        throw new Exception("Carta nao encontrada");
        
    }
}

