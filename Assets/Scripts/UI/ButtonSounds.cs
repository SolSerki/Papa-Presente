using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Sonido de hover y click de un botón. El AudioManager se lo agrega a todos los botones al cargar cada escena.
[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            AudioManager.PlayButtonHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
            AudioManager.PlayButtonClick();
    }
}
