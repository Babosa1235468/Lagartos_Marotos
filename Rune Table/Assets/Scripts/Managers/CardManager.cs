using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    [SerializeField] List<Card> Deck;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
    }
}