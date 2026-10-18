using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{

    [SerializeField] private SpriteRenderer frame;
    [SerializeField] private SpriteRenderer artwork;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private CardData initialData;

    [Header("Marcos por tipo")]
    [SerializeField] private CardFrames positiveFrames;
    [SerializeField] private CardFrames negativeFrames;
    [SerializeField] private CardFrames neutralFrames;
    [SerializeField] private CardFrames strategyFrames;

    private CardFrames frames;

    public CardData Data { get; private set; }

        public void Setup(CardData data)
    {
        Data = data;
        nameText.text = data.CardName;
        descriptionText.text = data.Description;
        artwork.sprite = data.Artwork;

        frames = GetFrames(data.Polarity);
        SetState(CardState.Idle);
    }

    public void SetState(CardState state)
    {
        if (frames == null)
            return;

        frame.sprite = frames.Get(state);
    }

    private CardFrames GetFrames(CardPolarity polarity)
    {
        switch (polarity)
        {
            case CardPolarity.Positive:
                return positiveFrames;
            case CardPolarity.Negative:
                return negativeFrames;
            case CardPolarity.Neutral:
                return neutralFrames;
            case CardPolarity.Strategy:
                return strategyFrames;
            default:
                return null;
        }
    }

    private void Start()
{
    if (initialData != null)
        Setup(initialData);
}
}
