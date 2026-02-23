using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public string cardText;
    public Sprite cardImage; 
    public CardEffect effectType;
    public CardLevel effectLevel;
    public float effectValue;
    public int unlockLevel;
    public bool isUnique;
}
public enum CardEffect
{
    Damage,
    Health,
    Movement,
    Gun,
    Special,
}
public enum CardLevel
{
    I,
    II,
    III,
}