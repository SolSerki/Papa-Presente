using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using System;

/// <summary>
/// Manages Dialogue flows Carga un CSV de diálogos al inicio y gestiona el flujo de conversación.
/// 
/// Use:
///   1. Assign a CSV as TextAsset in the Inspector (in csvFile).
///   2. It loads the CSV file at start 
///   3. call StartDialogue("1").
///   4. Call Continue() when 'Continue' button is pushed
///   5.  Calling TriggerResponse(bool) event when card dragged into response area will call next node
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("CSV dialogue file")]
    [SerializeField] private TextAsset csvFile;
    private string DEFAULT_INDEX = "1";

 
    /// <summary>Invoked to trigger a dialogue node.</summary>
    public event Action<DialogueNode> OnDialogueShow;

    /// <summary>trigger when conversation ends.</summary>
    public event Action OnDialogueEnd;

    //Dialogue state
    private Dictionary<string, DialogueNode> _nodes = new Dictionary<string, DialogueNode>();
    private DialogueNode _current;

    private void Awake()
    {
        LoadFromCSV();
    }

    // ------------------------------CSV Managment-------------------------------

    /// <summary>
    /// A simple CSV loader for dialogues.
    /// 
    /// Use: load row per row into nodes to be displayed sequencially 
    /// </summary>
    private void LoadFromCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("[DialogueManager] No CSV assigned. Asssign the TextAsset in the Inspector.");
            return;
        }

        _nodes.Clear();

        // Dividir en líneas (compatible con \n y \r\n)
        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Skip 1st row, headers
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] fields = ParseCSVLine(line);

            // expected columns: id, type, characterName, text, nextNodeId, presentId, absentId
            if (fields.Length < 6)
            {
                Debug.LogWarning($"[DialogueManager] row {i + 1} ignored. (less than 6 columns): {line}");
                continue;
            }

            DialogueNode node = new DialogueNode
            {
                id = fields[0].Trim(),
                type = fields[1].Trim(),
                characterName = fields[2].Trim(),
                text = fields[3].Trim(),
                nextNodeId = fields[4].Trim(),
                presentId = fields[5].Trim(),
                absentId = fields[6].Trim()
            };

            if (string.IsNullOrEmpty(node.id))
            {
                Debug.LogWarning($"[DialogueManager] row {i + 1} ignored: ID is empty.");
                continue;
            }
            if (string.IsNullOrEmpty(node.type) || (!Enum.TryParse<NodeType>(node.type, ignoreCase:false, out var nodeType) || 
                !Enum.IsDefined(typeof(NodeType), nodeType)))
            {
                Debug.LogWarning($"[DialogueManager] row {i + 1} ignored: type not valid.");
                continue;
            }

                if (_nodes.ContainsKey(node.id))
            {
                Debug.LogWarning($"[DialogueManager] duplicate ID '{node.id}' in row {i + 1}. Overwritten.");
            }

            _nodes[node.id] = node;
        }

        Debug.Log($"[DialogueManager] CSV loaded: {_nodes.Count} nodes.");
    }

    /// <summary>
    /// Parser CSV 
    /// </summary>
    private string[] ParseCSVLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Comilla escapada "" dentro de un campo
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; // saltear la segunda comilla
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString()); // último campo
        return fields.ToArray();
    }

    //-----------------------------------> CSV Management end

    /// <summary>
    /// </summary>
    /// <param name="startId"></param>
    public void StartDialogue(string startId)
    {
        if (string.IsNullOrEmpty(startId))
        {
            startId = DEFAULT_INDEX;
            Debug.LogWarning($"[DialogueManager] Start node set to default: '{DEFAULT_INDEX}'.");
        }
        if (_nodes.TryGetValue(startId, out DialogueNode node))
        {
            ShowNode(node);
        }
        else
        {
            Debug.LogWarning($"[DialogueManager] Node '{startId}' not found in CSV.");
        }
    }
    private void EndDialogue()
    {
        Debug.Log("[DialogueManager] End of conversation.");
        _current = null;
        OnDialogueEnd?.Invoke();
    }
    private void ShowNode(DialogueNode node)
    {
        _current = node;
        OnDialogueShow?.Invoke(node);

        Debug.Log($"[DialogueManager] Showing [{node.id} - {node.type}] {node.characterName}: {node.text}" +
                  (node.IsQuestion ? " [espera respuesta bool]" : ""));
    }

    /// <summary>
    /// Next node when 'continue' button is clicked
    /// It only works if current node is not a question (IsQuestion == false).
    /// </summary>
    public void Continue()
    {
        if (_current == null)
        {
            Debug.LogWarning("[DialogueManager] No active dialogue");
            return;
        }

        if (_current.IsQuestion)
        {
            Debug.LogWarning("[DialogueManager] Este nodo espera TriggerResponse(bool). Usá ese método.");
            return;
        }

        if (_current.IsEnd)
        {
            EndDialogue();
        }
        else
        {
            StartDialogue(_current.nextNodeId);
        }
    }
}
