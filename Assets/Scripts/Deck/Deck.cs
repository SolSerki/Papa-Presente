using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
[Header("Mazo fijo (si está vacío, se arma al azar)")]
[SerializeField] private List<CardData> startingCards;

[Header("Pools para el mazo al azar")]
[SerializeField] private List<CardData> positivePool;
[SerializeField] private List<CardData> negativePool;
[SerializeField] private List<CardData> neutralPool;

[SerializeField] private int positiveCount = 5;
[SerializeField] private int negativeCount = 5;
[SerializeField] private int neutralCount = 2;
    private List<CardData> drawPile = new List<CardData>();

    public int Count => drawPile.Count;

    private void Awake()
    {
        if (startingCards.Count > 0)
        {
            drawPile.AddRange(startingCards);
        }
        else
        {
            BuildRandomDeck();
        }
        Shuffle();
    }

    private void BuildRandomDeck()
    {
        AddRandomCards(positivePool, positiveCount);
        AddRandomCards(negativePool, negativeCount);
        AddRandomCards(neutralPool, neutralCount);
    }

private void AddRandomCards(List<CardData> pool, int amount)
    {
        if (pool.Count == 0)
            return;

        for (int i = 0; i < amount; i++)
            drawPile.Add(pool[Random.Range(0, pool.Count)]);
    }

    public void Shuffle()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (drawPile[i], drawPile[j]) = (drawPile[j], drawPile[i]);
        }
    }

    public CardData Draw()
    {
        if (drawPile.Count == 0)
            return null;

        CardData top = drawPile[drawPile.Count - 1];
        drawPile.RemoveAt(drawPile.Count - 1);
        return top;
    }
}
