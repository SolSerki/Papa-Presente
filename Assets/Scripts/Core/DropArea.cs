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

        // Las estrategias no responden al diálogo, se juegan en cualquier momento.
        CardView droppedCard = pointerData.pointerDrag.GetComponent<CardView>();
        if (droppedCard != null && droppedCard.Data != null && droppedCard.Data.Polarity == CardPolarity.Strategy)
        {
            Debug.Log("[DropArea] Estrategia jugada: " + droppedCard.Data.CardName + " (efecto pendiente)");
            hand.PlayCard(droppedCard);
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
