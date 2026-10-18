using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Frames")]
public class CardFrames : ScriptableObject
{
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite hover;
    [SerializeField] private Sprite pressed;

    public Sprite Get(CardState state)
    {
        switch (state)
        {
            case CardState.Hover:
                return hover;
            case CardState.Pressed:
                return pressed;
            default:
                return idle;
        }
    }
}
