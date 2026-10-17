using System;
using UnityEngine;

/// <summary>
/// One dialogue node (challenge)
/// </summary>
[Serializable]
public class DialogueNode
{
    public string id;

    public NodeType type; // A - B - NI

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


    public bool IsQuestion => !string.IsNullOrEmpty(presentId) || !string.IsNullOrEmpty(absentId);

    /// <summary>True if it's te last node in DialogueCSV.</summary>
    public bool IsEnd => string.IsNullOrEmpty(nextNodeId) && !IsQuestion;
}
