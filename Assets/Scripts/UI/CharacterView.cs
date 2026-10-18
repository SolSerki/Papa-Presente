using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Cambia el dibujo del personaje según quién habla en el diálogo
// (por ejemplo, la mano con el celular cuando es una llamada).
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

    private void OnEnable()
    {
        dialogueManager.OnDialogueShow += OnDialogueShow;
    }

    private void OnDisable()
    {
        dialogueManager.OnDialogueShow -= OnDialogueShow;
    }

    private void OnDialogueShow(DialogueNode node)
    {
        string speaker = Normalize(node.characterName);
        foreach (CharacterSprite character in characters)
        {
            if (Normalize(character.characterName) == speaker)
            {
                // Sin dibujo = no se muestra ningún personaje (por ejemplo, el narrador)
                image.sprite = character.sprite;
                image.enabled = character.sprite != null;
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
