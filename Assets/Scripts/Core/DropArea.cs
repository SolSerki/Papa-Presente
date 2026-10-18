using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    public void OnItemDropped(BaseEventData data)
    {
        // to access the dropped object
        PointerEventData pointerData = data as PointerEventData;

        if (pointerData == null) return;

        GameObject droppedObject = pointerData.pointerDrag;

        CardView cardView = droppedObject.GetComponent<CardView>();
        if (cardView != null && cardView.Data != null)
        {
            dialogueManager.TriggerResponse(cardView.Data.Polarity);
            Debug.Log("dropped: " + droppedObject.name);
        }
        else
        {
            Debug.LogWarning("[DropArea.OnItemDropped] No CardData assigned to Prefab, assign one in the inspector");
        }
    }
}
