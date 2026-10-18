using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Música de cada pantalla y efectos de sonido. Sobrevive entre escenas, igual que el GameManager.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Fuentes")]
    [SerializeField] private AudioSource musicSource;
    [Tooltip("Arranca el loop del juego justo cuando termina la transición del menú.")]
    [SerializeField] private AudioSource nextMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip transitionToGameMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    [Header("Botones")]
    [SerializeField] private AudioClip buttonHover;
    [SerializeField] private AudioClip buttonClick;

    [Header("Cartas")]
    [SerializeField] private AudioClip cardHover;
    [SerializeField] private AudioClip cardSelect;
    [SerializeField] private AudioClip cardDraw;
    [SerializeField] private AudioClip cardDiscard;

    [Header("Volumen por defecto (la primera vez que se juega)")]
    [SerializeField, Range(0f, 1f)] private float defaultMusicVolume = 0.6f;
    [SerializeField, Range(0f, 1f)] private float defaultSfxVolume = 1f;

    private const string MUSIC_VOLUME_KEY = "musicVolume";
    private const string SFX_VOLUME_KEY = "sfxVolume";

    // La música que se pidió por última vez (no depende de isPlaying, que da false en pausa o mientras carga)
    private AudioClip currentMusic;
    private float musicVolume;
    private float sfxVolume;

    // Se guardan en PlayerPrefs, así el volumen elegido queda para la próxima vez que se abra el juego
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
            ApplyVolumes();
        }
    }

    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
            ApplyVolumes();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSfxVolume);
        ApplyVolumes();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void ApplyVolumes()
    {
        musicSource.volume = musicVolume;
        nextMusicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddButtonSounds();

        // Settings se abre encima de la escena actual: la música sigue como estaba
        if (mode == LoadSceneMode.Additive)
            return;

        if (scene.name == GameManager.SCENE_1)
        {
            PlayGameplayMusic();
        }
        else if (scene.name == GameManager.RUN_END_SCENE)
        {
            bool present = GameManager.Instance != null && GameManager.Instance.LastRunPresent;
            PlayOnce(present ? winMusic : loseMusic);
        }
        else
        {
            // MainMenu y Credits comparten la música, así no se corta al ir y volver
            PlayLoop(menuMusic);
        }
    }

    // ------------------------------ Música ------------------------------

    private void PlayGameplayMusic()
    {
        bool comesFromMenu = currentMusic == menuMusic;
        if (!comesFromMenu || transitionToGameMusic == null)
        {
            PlayLoop(gameplayMusic);
            return;
        }

        // Transición una vez y, cuando termina, el loop del juego sin cortes
        musicSource.clip = transitionToGameMusic;
        musicSource.loop = false;
        musicSource.Play();

        double transitionLength = (double)transitionToGameMusic.samples / transitionToGameMusic.frequency;
        nextMusicSource.clip = gameplayMusic;
        nextMusicSource.loop = true;
        nextMusicSource.PlayScheduled(AudioSettings.dspTime + transitionLength);
        currentMusic = gameplayMusic;
    }

    private void PlayLoop(AudioClip clip)
    {
        if (currentMusic == clip)
            return;

        nextMusicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        currentMusic = clip;
    }

    private void PlayOnce(AudioClip clip)
    {
        nextMusicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = false;
        musicSource.Play();
        currentMusic = clip;
    }

    // ------------------------------ Efectos ------------------------------

    // Todos los botones de la escena suenan al pasar el mouse y al hacer click
    private void AddButtonSounds()
    {
        foreach (Button button in FindObjectsByType<Button>(FindObjectsInactive.Include))
        {
            if (button.GetComponent<ButtonSounds>() == null)
                button.gameObject.AddComponent<ButtonSounds>();
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Se pueden llamar desde cualquier script; si no hay AudioManager (probando una escena suelta) no hacen nada
    public static void PlayButtonHover() { if (Instance != null) Instance.PlaySfx(Instance.buttonHover); }
    public static void PlayButtonClick() { if (Instance != null) Instance.PlaySfx(Instance.buttonClick); }
    public static void PlayCardHover() { if (Instance != null) Instance.PlaySfx(Instance.cardHover); }
    public static void PlayCardSelect() { if (Instance != null) Instance.PlaySfx(Instance.cardSelect); }
    public static void PlayCardDraw() { if (Instance != null) Instance.PlaySfx(Instance.cardDraw); }
    public static void PlayCardDiscard() { if (Instance != null) Instance.PlaySfx(Instance.cardDiscard); }
}
