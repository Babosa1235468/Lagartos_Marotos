using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

[Serializable]
public class Effects
{
    public List<Effect> effects { get; set; }
    public int rarity { get; set; }

    public Effects()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Effects");
        string json = jsonFile.text;

        Effects data = JsonUtility.FromJson<Effects>(json);

        effects = data.effects;
        rarity = data.rarity;
    }

    public Effect getRandomEffect()
    {
        int ranNum = Random.Range(1, 101);

        if (ranNum > rarity) return null;

        int totalRarity = 0;
        foreach (var eff in effects)
        {
            totalRarity += eff.rarity;
        }
        ranNum = Random.Range(1, totalRarity + 1);
        int actualRarity = 0;
        foreach (var eff in effects)
        {
            actualRarity += eff.rarity;
            if (actualRarity >= ranNum)
            {
                return eff;
            }
        }
        return null;
    }

}
[Serializable]
public class Effect
{
    public string name { get; set; }
    public string description { get; set; }
    public int rarity { get; set; }
    public int priceAdd { get; set; }
}