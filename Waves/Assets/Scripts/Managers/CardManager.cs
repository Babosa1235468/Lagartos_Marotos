
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject cardSelectionUI;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardPositionOne;
    [SerializeField] Transform cardPositionTwo;
    [SerializeField] Transform cardPositionThree;
    [SerializeField] Transform cardPositionFour;
    [SerializeField] List<CardSO> deck;


    // Currently randomized cards
    GameObject cardOne, cardTwo, cardThree, cardFour;

    List<CardSO> alreadySelectedCards = new List<CardSO>();



    // --------------------- MAKE THIS A SINGLETON ---------------------
    private void Awake()
    {
        instance = this;
    }

    // --------------------- DOES NOTHING RN ---------------------
    private void Start()
    {
        //RandomizeNewCards();
    }

    // ------------------ RANDOMIZE CARDS FOR NOW ---------------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            RandomizeNewCards();
        }

    }

    // --------------------- RANDOMIZE CARDS  -------------------
    public void RandomizeNewCards()
    {
        //Destroy previous sorted cards
        if (cardOne != null) Destroy(cardOne);
        if (cardTwo != null) Destroy(cardTwo);
        if (cardThree != null) Destroy(cardThree);
        if (cardFour != null) Destroy(cardFour);

        List<CardSO> RandomizedCards = new List<CardSO>();

        List<CardSO> avaliableCards = new List<CardSO>(deck);
        avaliableCards.RemoveAll(card => card.isUnique
            && alreadySelectedCards.Contains(card)
            || card.unlockLevel > GameManager.instance.CurrentLevel()
        );
        if (avaliableCards.Count < 4)
        {
            Debug.Log("Not Enough Avaliable Cards");
            return;
        }
        while (RandomizedCards.Count < 4)
        {
            CardSO randomCard = avaliableCards[Random.Range(0, avaliableCards.Count)];
            if (!RandomizedCards.Contains(randomCard))
            {
                RandomizedCards.Add(randomCard);
            }
        }

        cardOne = InstantiateCard(RandomizedCards[0], cardPositionOne);
        cardTwo = InstantiateCard(RandomizedCards[1], cardPositionTwo);
        cardThree = InstantiateCard(RandomizedCards[2], cardPositionThree);
        cardFour = InstantiateCard(RandomizedCards[3], cardPositionFour);
        ShowCardSelection();
    }

    // --------------------- CREATE THE CARDS -------------------
    GameObject InstantiateCard(CardSO cardSO, Transform position)
    {
        GameObject cardGO = Instantiate(cardPrefab, position.position, Quaternion.identity, position);
        Card card = cardGO.GetComponent<Card>();
        card.Setup(cardSO);
        return cardGO;
    }

    // --------------------- SELECT THE CARDS ---------------------
    public void SelectCard(CardSO selectedCard)
    {
        if (!alreadySelectedCards.Contains(selectedCard))
        {
            alreadySelectedCards.Add(selectedCard);
        }
        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }

    // --------------------- HIDE/SHOW -------------------
    public void ShowCardSelection()
    {
        cardSelectionUI.SetActive(true);
    }
    public void HideCardSelection()
    {
        cardSelectionUI.SetActive(false);
    }
}
