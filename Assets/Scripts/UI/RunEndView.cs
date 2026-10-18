using TMPro;
using UnityEngine;

// Muestra en la escena RunEnd si fue papá presente o ausente y la carta que se ganó.
public class RunEndView : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CardView rewardCard;

    [Header("Mensajes")]
    [SerializeField, TextArea(2, 4)] private string presentMessage =
        "¡Estuviste presente!\nGanaste una carta positiva para tu mazo.";
    [SerializeField, TextArea(2, 4)] private string absentMessage =
        "¡Ey, parece que aprendiste algo!\nGanaste una carta de estrategia, con ella la próxima te será más fácil.";

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogWarning("[RunEndView] No hay GameManager, arrancá desde MainMenu o Scene1.");
            rewardCard.gameObject.SetActive(false);
            return;
        }

        messageText.text = gameManager.LastRunPresent ? presentMessage : absentMessage;

        if (gameManager.LastReward != null)
            rewardCard.Setup(gameManager.LastReward);
        else
            rewardCard.gameObject.SetActive(false);
    }
}
