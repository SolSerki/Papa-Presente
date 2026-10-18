using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Pantalla de ajustes: volumen de la música y de los efectos.
public class SettingsView : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Tooltip("Se oculta si los ajustes se abren desde el menú principal.")]
    [SerializeField] private GameObject mainMenuButton;

    [Tooltip("Arranca apagado. Solo se prende si la escena se abre sola, sin otra que ya tenga EventSystem.")]
    [SerializeField] private GameObject eventSystemObject;

    private float nextSfxPreview;

    private void Awake()
    {
        if (EventSystem.current == null)
            eventSystemObject.SetActive(true);
    }

    private void Start()
    {
        // Desde el menú, "menú principal" no tiene sentido
        if (SceneManager.GetActiveScene().name == GameManager.MAIN_MENU_SCENE)
            mainMenuButton.SetActive(false);

        AudioManager audioManager = AudioManager.Instance;
        if (audioManager != null)
        {
            musicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
            sfxSlider.SetValueWithoutNotify(audioManager.SfxVolume);
        }

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    private void OnMusicChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.MusicVolume = value;
    }

    private void OnSfxChanged(float value)
    {
        if (AudioManager.Instance == null)
            return;

        AudioManager.Instance.SfxVolume = value;

        // Suena un efecto para escuchar el volumen, pero no en cada cuadro mientras se arrastra.
        // Se usa unscaledTime porque con Settings abierto el juego está pausado.
        if (Time.unscaledTime >= nextSfxPreview)
        {
            AudioManager.PlayCardSelect();
            nextSfxPreview = Time.unscaledTime + 0.15f;
        }
    }
}
