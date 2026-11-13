using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Cambia el dibujo del personaje según quién habla en el diálogo
// (por ejemplo, la mano con el celular cuando es una llamada).
// Cuando el jugador le responde a la hija, ella reacciona con otra cara.
public class CharacterView : MonoBehaviour
{
    [Serializable]
    private class CharacterSprite
    {
        [Tooltip("Como figura en la columna characterName del CSV (no importan mayúsculas ni comillas).")]
        public string characterName;
        public Sprite sprite;
    }

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Image image;
    [Tooltip("Si quien habla no está en la lista, queda el dibujo que estaba. Un personaje sin dibujo oculta la imagen.")]
    [SerializeField] private List<CharacterSprite> characters = new List<CharacterSprite>();

    [Header("Reacciones a las respuestas")]
    [Tooltip("Personaje que reacciona cuando le respondés (como figura en el CSV).")]
    [SerializeField] private string reactingCharacter = "HIJA";
    [SerializeField] private Sprite goodReaction;
    [SerializeField] private Sprite neutralReaction;
    [SerializeField] private Sprite badReaction;
    [Tooltip("Se usa en lugar de la mala cuando hay 2 respuestas malas seguidas.")]
    [SerializeField] private Sprite badStreakReaction;

    private string lastSpeaker = "";
    private int badStreak;
    // Queda la cara de la última respuesta que se le dio, hasta que llegue otra
    private Sprite reactionSprite;

    private void OnEnable()
    {
        dialogueManager.OnDialogueShow += OnDialogueShow;
        dialogueManager.OnResponse += OnResponse;
    }

    private void OnDisable()
    {
        dialogueManager.OnDialogueShow -= OnDialogueShow;
        dialogueManager.OnResponse -= OnResponse;
    }

    private void OnResponse(CardPolarity polarity)
    {
        if (polarity == CardPolarity.Negative)
            badStreak++;
        else if (polarity == CardPolarity.Positive || polarity == CardPolarity.Neutral)
            badStreak = 0;

        // Solo reacciona si la pregunta se la hizo ella
        if (lastSpeaker != Normalize(reactingCharacter))
            return;

        Sprite reaction = PickReaction(polarity);
        if (reaction != null)
            reactionSprite = reaction;
    }

    private Sprite PickReaction(CardPolarity polarity)
    {
        switch (polarity)
        {
            case CardPolarity.Positive:
                return goodReaction;
            case CardPolarity.Neutral:
                return neutralReaction;
            case CardPolarity.Negative:
                return badStreak >= 2 && badStreakReaction != null ? badStreakReaction : badReaction;
            default:
                return null;
        }
    }

    private void OnDialogueShow(DialogueNode node)
    {
        string speaker = Normalize(node.characterName);
        lastSpeaker = speaker;
        bool isReacting = speaker == Normalize(reactingCharacter) && reactionSprite != null;

        foreach (CharacterSprite character in characters)
        {
            if (Normalize(character.characterName) == speaker)
            {
                // Sin dibujo = no se muestra ningún personaje (por ejemplo, el narrador)
                Sprite sprite = isReacting ? reactionSprite : character.sprite;
                image.sprite = sprite;
                image.enabled = sprite != null;
                return;
            }
        }
    }

    // En el CSV algunos nombres vienen entre comillas, como "NÚMERO DESCONOCIDO"
    private static string Normalize(string name)
    {
        return string.IsNullOrEmpty(name) ? "" : name.Trim().Trim('"').Trim().ToUpperInvariant();
    }
}
