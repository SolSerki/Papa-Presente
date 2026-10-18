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
    [Tooltip("Radio de la curva del tablero, en unidades locales de la mano. 0 = cartas en línea recta.")]
    [SerializeField] private float curveRadius = 26f;

    [Header("Descarte")]
    [SerializeField] private DiscardPile discardPile;

    [Header("Estrategia")]
    [Tooltip("Cartel 'Seleccioná una carta para descartar'.")]
    [SerializeField] private GameObject discardPrompt;

    private readonly List<CardView> cards = new List<CardView>();

    public int Count => cards.Count;

    // Mientras es true, hacer click en una carta de la mano la descarta (no se puede jugar)
    public bool IsSelectingDiscard { get; private set; }

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
        AudioManager.PlayCardDraw();
        return true;
    }

    private void ArrangeCards()
    {
        float startX = -(cards.Count - 1) * spacing / 2f;
        for (int i = 0; i < cards.Count; i++)
            PlaceOnCurve(cards[i].transform, startX + i * spacing);
    }

    // Ubica la carta sobre un arco de círculo cuyo centro está abajo de la mano.
    // x es la distancia medida sobre el arco desde el centro de la mano.
    private void PlaceOnCurve(Transform card, float x)
    {
        if (curveRadius <= 0f)
        {
            card.localPosition = new Vector3(x, 0f, 0f);
            card.localRotation = Quaternion.identity;
            return;
        }

        float angle = x / curveRadius; // en radianes
        card.localPosition = new Vector3(Mathf.Sin(angle) * curveRadius, (Mathf.Cos(angle) - 1f) * curveRadius, 0f);
        card.localRotation = Quaternion.Euler(0f, 0f, -angle * Mathf.Rad2Deg);
    }

    public void PlayCard(CardView card)
    {
        if (!cards.Remove(card))
            return;

        Discard(card);
        AudioManager.PlayCardDiscard();
        ArrangeCards();
        DrawCard();
    }

    // Las estrategias no responden al diálogo: hacen su efecto en el momento
    public void PlayStrategy(CardView card)
    {
        if (IsSelectingDiscard || !cards.Remove(card))
            return;

        StrategyEffect effect = card.Data.StrategyEffect;
        Discard(card);
        AudioManager.PlayCardDiscard();

        switch (effect)
        {
            case StrategyEffect.DiscardHandAndDraw:
                foreach (CardView other in cards)
                    Discard(other);
                cards.Clear();
                DrawCards(maxHandSize);
                break;

            case StrategyEffect.ChooseDiscardAndDraw:
                // Se roba después de elegir: la que reemplaza a la estrategia y la descartada
                if (cards.Count == 0)
                    DrawCards(maxHandSize);
                else
                    StartDiscardSelection();
                break;

            default:
                ArrangeCards();
                DrawCard();
                break;
        }
    }

    private void StartDiscardSelection()
    {
        IsSelectingDiscard = true;
        ArrangeCards();
        if (discardPrompt != null)
            discardPrompt.SetActive(true);
    }

    // Lo llama CardInteraction al hacer click en una carta mientras se elige el descarte
    public void SelectForDiscard(CardView card)
    {
        if (!IsSelectingDiscard || !cards.Remove(card))
            return;

        IsSelectingDiscard = false;
        if (discardPrompt != null)
            discardPrompt.SetActive(false);

        Discard(card);
        AudioManager.PlayCardDiscard();
        ArrangeCards();
        DrawCards(maxHandSize);
    }

    private void Discard(CardView card)
    {
        discardPile.Add(card.Data);
        Destroy(card.gameObject);
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            ArrangeCards();
    }

    [ContextMenu("Test Draw Card")]
    private void TestDrawCard() => DrawCard();
}
