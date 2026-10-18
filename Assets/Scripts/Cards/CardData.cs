using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{

[SerializeField] private string cardName;
[SerializeField, TextArea(2, 4)] private string description;
[SerializeField] private Sprite artwork;
[SerializeField] private CardPolarity polarity;
[Tooltip("Solo para cartas de estrategia.")]
[SerializeField] private StrategyEffect strategyEffect;


public string CardName => cardName;
public string Description => description;
public Sprite Artwork => artwork;
public CardPolarity Polarity => polarity;
public StrategyEffect StrategyEffect => strategyEffect;



}
