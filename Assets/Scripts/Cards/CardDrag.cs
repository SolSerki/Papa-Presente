using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Collider2D cardCollider;
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private int dragSortingOrder = 100;

    private Camera cam;
    private Vector3 startLocalPosition;
    private int startSortingOrder;
    private Vector3 grabOffset;
    private DropArea dropArea;
    private CardView cardView;

    private void Awake()
    {
        cam = Camera.main;
        // La carta sale de un prefab, así que el DropArea de la escena se busca acá
        dropArea = FindAnyObjectByType<DropArea>();
        cardView = GetComponent<CardView>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startLocalPosition = transform.localPosition;
        startSortingOrder = sortingGroup.sortingOrder;

        sortingGroup.sortingOrder = dragSortingOrder;
        cardCollider.enabled = false;

        grabOffset = transform.position - PointerToWorld(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = PointerToWorld(eventData) + grabOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = startLocalPosition;
        sortingGroup.sortingOrder = startSortingOrder;
        cardCollider.enabled = true;

        // Soltarla fuera del tablero la juega
        if (dropArea != null && dropArea.IsOutsideBoard(eventData.position))
            dropArea.PlayDroppedCard(cardView);
    }

    private Vector3 PointerToWorld(PointerEventData eventData)
    {
        Vector3 world = cam.ScreenToWorldPoint(eventData.position);
        world.z = transform.position.z;
        return world;
    }
}