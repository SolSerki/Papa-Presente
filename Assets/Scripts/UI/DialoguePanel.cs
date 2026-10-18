using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Narrador")]
    [Tooltip("Etiqueta del nombre: se oculta cuando habla el narrador.")]
    [SerializeField] private GameObject nameLabel;
    [Tooltip("Como figura en la columna characterName del CSV.")]
    [SerializeField] private string narratorName = "CAJA DE TEXTO";

    [Header("Configuración")]
    [Tooltip("ID del primer nodo al iniciar la escena.")]
    [SerializeField] private string startNodeId = "1"; 

    private void Start() 
    {
        // Suscribirse a los eventos del Manager
        dialogueManager.OnDialogueShow += OnDialogueShow;
        dialogueManager.OnDialogueEnd += OnDialogueEnd;

        // Conectar el botón Continuar
        continueButton.onClick.AddListener(dialogueManager.Continue);

        // Comenzar la conversación
        dialogueManager.StartDialogue(startNodeId);
    }
    private void OnDialogueShow(DialogueNode node)
    {
        if (characterNameText != null)
            characterNameText.text = node.characterName;

        // El narrador (intro y outros) no lleva etiqueta de nombre
        if (nameLabel != null)
            nameLabel.SetActive(node.characterName != narratorName);
        dialogueText.text = node.text;

        // hide 'continue' button if dialogue expects a card
        // Button will show in next node (OnDialogueShow will be invoked again)
        continueButton.gameObject.SetActive(!node.IsQuestion);
    }

    private void OnDialogueEnd()
    {
        
<<<<<<< HEAD
        gameManager.FinishRun();
=======
        gameObject.SetActive(false);
        // Se usa Instance porque sobrevive el GameManager de la primera escena cargada
        GameManager.Instance.FinishRun(dialogueManager.IsPresentEnding);
>>>>>>> refs/remotes/origin/main
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueShow -= OnDialogueShow;
            dialogueManager.OnDialogueEnd -= OnDialogueEnd;
        }
        continueButton.onClick.RemoveListener(dialogueManager.Continue);
    }
}
