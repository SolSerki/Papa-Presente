using UnityEngine;
using UnityEngine.EventSystems;

// Cambia el marco de la carta según el mouse: idle, hover y pressed (al hacer click y mientras se arrastra).
public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private CardView view;

    private bool isHovered;
    private bool isDragging;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (!isDragging)
            view.SetState(CardState.Hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // CardDrag apaga el collider al arrastrar, eso dispara un Exit que hay que ignorar.
        isHovered = false;
        if (!isDragging)
            view.SetState(CardState.Idle);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        view.SetState(CardState.Pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging)
            view.SetState(isHovered ? CardState.Hover : CardState.Idle);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        view.SetState(CardState.Pressed);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // La carta vuelve a su lugar en la mano, lejos del mouse.
        isDragging = false;
        isHovered = false;
        view.SetState(CardState.Idle);
    }
}
