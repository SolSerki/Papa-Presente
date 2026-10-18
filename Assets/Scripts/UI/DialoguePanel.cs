using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

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
        //characterNameText.text = node.characterName;
        dialogueText.text = node.text;

        // hide 'continue' button if dialogue expects a card
        // Button will show in next node (OnDialogueShow will be invoked again)
        continueButton.gameObject.SetActive(!node.IsQuestion);
    }

    private void OnDialogueEnd()
    {
        
        gameObject.SetActive(false);
        gameManager.FinishRun();
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
