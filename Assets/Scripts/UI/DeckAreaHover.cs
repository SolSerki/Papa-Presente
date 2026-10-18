using UnityEngine;
using UnityEngine.InputSystem;

// Deja la zona de abajo (tablero, mazo, descarte y cartas de la mano) más abajo
// y la levanta cuando el mouse se acerca.
public class DeckAreaHover : MonoBehaviour
{
    [SerializeField] private RectTransform deckArea;
    [SerializeField] private Transform hand;

    [Header("Movimiento (en unidades de mundo)")]
    [SerializeField] private float loweredOffset = 1.1f;
    [Tooltip("Cuánto por encima del tablero cuenta como 'mouse encima', porque las cartas sobresalen.")]
    [SerializeField] private float hoverMargin = 0.6f;
    [SerializeField] private float speed = 10f;

    private readonly Vector3[] corners = new Vector3[4];
    private Camera cam;
    private float deckAreaRaisedY;
    private float handRaisedY;
    private float currentOffset;

    private void Awake()
    {
        cam = Camera.main;
        deckAreaRaisedY = deckArea.anchoredPosition.y;
        handRaisedY = hand.localPosition.y;

        // Arranca abajo
        currentOffset = loweredOffset;
        Apply();
    }

    private void Update()
    {
        float target = IsMouseOverArea() ? 0f : loweredOffset;
        currentOffset = Mathf.Lerp(currentOffset, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
        Apply();
    }

    private bool IsMouseOverArea()
    {
        if (Mouse.current == null)
            return false;

        Vector3 mouse = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Se mide contra el borde de arriba actual: abajo hay que acercarse más para que suba,
        // y una vez arriba no baja mientras el mouse siga sobre las cartas.
        deckArea.GetWorldCorners(corners);
        float top = corners[1].y + hoverMargin;
        return mouse.y < top;
    }

    private void Apply()
    {
        // El Canvas está escalado, así que el desplazamiento se pasa a sus unidades
        float canvasOffset = currentOffset / deckArea.lossyScale.y;
        deckArea.anchoredPosition = new Vector2(deckArea.anchoredPosition.x, deckAreaRaisedY - canvasOffset);
        hand.localPosition = new Vector3(hand.localPosition.x, handRaisedY - currentOffset, hand.localPosition.z);
    }
}
