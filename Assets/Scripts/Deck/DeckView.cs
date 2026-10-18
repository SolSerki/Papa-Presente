using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Deck deck;
    [SerializeField] private TMP_Text countText;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite hoverSprite;

    private void Start()
    {
        countText.gameObject.SetActive(false);
        spriteRenderer.sprite = idleSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        countText.text = $"{deck.Count} cartas";
        countText.gameObject.SetActive(true);
        spriteRenderer.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        countText.gameObject.SetActive(false);
        spriteRenderer.sprite = idleSprite;
    }
}
