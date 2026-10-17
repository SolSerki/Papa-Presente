using System;
using UnityEngine;

/// <summary>
/// One dialogue node (challenge)
/// </summary>
[Serializable]
public class DialogueNode
{
    public string id;

    public string type; // A - B - NI

    public string characterName;

    public string text;

    /// <summary>
    /// Empty if this node expects a bool to trigger nextTrue/nextFalse
    /// </summary>
    public string nextNodeId;

    /// <summary>node id of present response line if DialogueManager.TriggerResponse receives present == true.</summary>
    public string presentId;

    /// <summary>node id of absent response line if DialogueManager.TriggerResponse receives present == false.</summary>
    public string absentId;

    /// <summary>
    /// True si este nodo necesita un evento booleano para avanzar
    /// (en lugar del botón Continuar).
    /// </summary>
    public bool IsQuestion => !string.IsNullOrEmpty(presentId) || !string.IsNullOrEmpty(absentId);

    /// <summary>True si es el último nodo de la conversación.</summary>
    public bool IsEnd => string.IsNullOrEmpty(nextNodeId) && !IsQuestion;
}
