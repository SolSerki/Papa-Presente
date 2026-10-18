using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
[SerializeField] private Deck deck;
    [SerializeField] private CardView cardPrefab;

    [Header("Reglas")]
    [SerializeField] private int startingHandSize = 3;
    [SerializeField] private int maxHandSize = 3;

    [Header("Layout")]
    [SerializeField] private float spacing = 2.8f;

    [Header("Descarte")]
    [SerializeField] private DiscardPile discardPile;
    private readonly List<CardView> cards = new List<CardView>();

    public int Count => cards.Count;

    private void Start()
    {
        DrawCards(startingHandSize);
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!DrawCard())
                break;
        }
    }

    public bool DrawCard()
    {
        if (cards.Count >= maxHandSize)
            return false;

        CardData data = deck.Draw();
        if (data == null)
            return false;

        CardView view = Instantiate(cardPrefab, transform);
        view.Setup(data);
        cards.Insert(0, view);
        ArrangeCards();
        return true;
    }

    private void ArrangeCards()
    {
        float startX = -(cards.Count - 1) * spacing / 2f;
        for (int i = 0; i < cards.Count; i++)
            cards[i].transform.localPosition = new Vector3(startX + i * spacing, 0f, 0f);
    }

    public void PlayCard(CardView card)
    {
        if (!cards.Remove(card))
            return;

        discardPile.Add(card.Data);
        Destroy(card.gameObject);
        ArrangeCards();
        DrawCard();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            ArrangeCards();
    }

    [ContextMenu("Test Draw Card")]
    private void TestDrawCard() => DrawCard();
}
