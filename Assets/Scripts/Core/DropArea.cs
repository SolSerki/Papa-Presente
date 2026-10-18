using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [Header("Player Hand")]
    [SerializeField] private PlayerHand hand;
    public void OnItemDropped(BaseEventData data)
    {
        // to access the dropped object
        PointerEventData pointerData = data as PointerEventData;
        if (pointerData == null) return;

        // Mientras una estrategia pide elegir una carta para descartar, no se juega nada
        if (hand.IsSelectingDiscard)
        {
            Debug.Log("[DropArea] Primero hay que elegir una carta para descartar.");
            return;
        }

        // Las estrategias no responden al diálogo, se juegan en cualquier momento.
        CardView droppedCard = pointerData.pointerDrag.GetComponent<CardView>();
        if (droppedCard != null && droppedCard.Data != null && droppedCard.Data.Polarity == CardPolarity.Strategy)
        {
            hand.PlayStrategy(droppedCard);
            return;
        }

        if (!dialogueManager.CanReceiveCard)
        {
        Debug.Log("[DropArea] El diálogo no espera una carta ahora.");
        return;
        }
        
        GameObject droppedObject = pointerData.pointerDrag;

        CardView cardView = droppedObject.GetComponent<CardView>();
        if (cardView != null && cardView.Data != null)
        {
            Debug.Log($"[OnItemDropped] Card polarity {cardView.Data.Polarity}");
            dialogueManager.TriggerResponse(cardView.Data.Polarity);
            hand.PlayCard(cardView);
            Debug.Log("[OnItemDropped] dropped: " + droppedObject.name);
        }
        else
        {
            Debug.LogWarning("[DropArea.OnItemDropped] No CardData assigned to Prefab, assign one in the inspector");
        }
    }
}
