using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Deck deck;
    [SerializeField] private TMP_Text countText;

    private void Start()
    {
        countText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        countText.text = $"{deck.Count} cartas";
        countText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        countText.gameObject.SetActive(false);
    }
}