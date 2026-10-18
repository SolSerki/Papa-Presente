using UnityEngine;
using System.Collections.Generic;

public class DiscardPile : MonoBehaviour
{
    private readonly List<CardData> cards = new List<CardData>();

    public int Count => cards.Count;

    public void Add(CardData card)
    {
        cards.Add(card);
    }

    public List<CardData> Reshuffle()
    {
        List<CardData> taken = new List<CardData>(cards);
        cards.Clear();
        return taken;
    }
}
