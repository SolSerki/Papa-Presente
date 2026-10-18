using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private DiscardPile discardPile;
    [SerializeField] private TMP_Text countText;

    private void Start()
    {
        countText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int count = discardPile.Count;
        countText.text = count == 1 ? "1 carta" : $"{count} cartas";
        countText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        countText.gameObject.SetActive(false);
    }
}
