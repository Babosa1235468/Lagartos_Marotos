using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
class Card
{
    public int life { get; set; }
    public int damage { get; set; }
    public int priceSacrifice { get; set; }
    public List<Effect> allEffects = new List<Effect>();

    public Card(int cardIndex)
    {
        //Carrega o Json
        TextAsset jsonFile = Resources.Load<TextAsset>("Cards");
        string json = jsonFile.text;

        
        List<Card> cards = JsonUtility.FromJson<List<Card>>(json);

        if (cards == null || cardIndex < 0 || cardIndex >= cards.Count)
            throw new Exception("Card inválido ou índice fora do alcance");

        Card card = cards[cardIndex];

        life = card.life;
        damage = card.damage;
        priceSacrifice = card.priceSacrifice;
    }
}
