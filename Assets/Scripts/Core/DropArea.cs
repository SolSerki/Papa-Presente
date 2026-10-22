using UnityEngine;

public class DropArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [Header("Player Hand")]
    [SerializeField] private PlayerHand hand;
    [Header("Tablero")]
    [Tooltip("Si la carta se suelta fuera de este rect, se juega.")]
    [SerializeField] private RectTransform board;

    public bool IsOutsideBoard(Vector2 screenPosition)
    {
        return !RectTransformUtility.RectangleContainsScreenPoint(board, screenPosition, Camera.main);
    }

    // Lo llama CardDrag al soltar una carta fuera del tablero
    public void PlayDroppedCard(CardView droppedCard)
    {
        // Mientras una estrategia pide elegir una carta para descartar, no se juega nada
        if (hand.IsSelectingDiscard)
        {
            Debug.Log("[DropArea] Primero hay que elegir una carta para descartar.");
            return;
        }

        if (droppedCard == null || droppedCard.Data == null)
        {
            Debug.LogWarning("[DropArea.PlayDroppedCard] No CardData assigned to Prefab, assign one in the inspector");
            return;
        }

        // Las estrategias no responden al diálogo, se juegan en cualquier momento.
        if (droppedCard.Data.Polarity == CardPolarity.Strategy)
        {
            hand.PlayStrategy(droppedCard);
            return;
        }

        if (!dialogueManager.CanReceiveCard)
        {
            Debug.Log("[DropArea] El diálogo no espera una carta ahora.");
            return;
        }

        Debug.Log($"[PlayDroppedCard] Card polarity {droppedCard.Data.Polarity}");
        dialogueManager.TriggerResponse(droppedCard.Data.Polarity);
        hand.PlayCard(droppedCard);
        Debug.Log("[PlayDroppedCard] dropped: " + droppedCard.name);
    }
}
