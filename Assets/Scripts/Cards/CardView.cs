using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{

    [SerializeField] private SpriteRenderer artwork;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private CardData initialData;

    public CardData Data { get; private set; }

        public void Setup(CardData data)
    {
        Data = data;
        nameText.text = data.CardName;
        descriptionText.text = data.Description;
        artwork.sprite = data.Artwork;
    }
    
    private void Start()
{
    if (initialData != null)
        Setup(initialData);
}
}
