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

        // Ya tenés el objeto, buscás el componente que necesites
        // por ejemplo:
        // DialogueItem item = droppedObject.GetComponent<DialogueItem>();

        Debug.Log("dropped: " + droppedObject.name);
    }
}
