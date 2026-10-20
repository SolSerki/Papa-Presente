using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }

    [Header("Recompensas al final de la run")]
    [SerializeField] private List<CardData> positiveRewards;
    [SerializeField] private List<CardData> strategyRewards;

    // Cartas ganadas en runs anteriores, se suman al mazo en cada run nueva
    private readonly List<CardData> wonCards = new List<CardData>();

    public IReadOnlyList<CardData> WonCards => wonCards;
    public bool LastRunPresent { get; private set; }
    public CardData LastReward { get; private set; }

    public static string SCENE_1 = "Scene1";
    public static string MAIN_MENU_SCENE = "MainMenu";
    public static string RUN_END_SCENE = "RunEnd";
    public static string CREDITS_SCENE = "Credits";
    public static string SETTINGS_SCENE = "Settings";
    public static string WIN_SCENE = "WinScene";
    public static string LOSE_SCENE = "LoseScene";

    void Start()
    {
        // add a line to load MainMenu
    }

    private void Awake()
    {
        StartSingleton();
    }

    private void StartSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame()
    {
        ResetProgress();
        ChangeScene(MAIN_MENU_SCENE);

    }

    /// <summary>
    /// Papá presente gana una carta positiva, papá ausente una de estrategia.
    /// </summary>
    public void FinishRun(bool present)
    {
        LastRunPresent = present;
        LastReward = PickRandom(present ? positiveRewards : strategyRewards);

        if (LastReward != null)
            wonCards.Add(LastReward);
        else
            Debug.LogWarning("[GameManager] No won cards loaded for this ending.");

        ChangeScene(RUN_END_SCENE);
    }

    public void ContinueGameLoop()
    {
        // La carta ganada ya está en wonCards, el mazo la suma al armarse
        ChangeScene(SCENE_1);
    }

    public void StartNewGame()
    {
        ResetProgress();
        ChangeScene(SCENE_1);
    }

    private void ResetProgress()
    {
        wonCards.Clear();
        LastReward = null;
        RelationshipManager.Instance.LoadInitialValues();
    }

    private CardData PickRandom(List<CardData> pool)
    {
        if (pool == null || pool.Count == 0)
            return null;

        return pool[Random.Range(0, pool.Count)];
    }

    public void ChangeScene(string name)
    {
        Time.timeScale = 1f; // por si se cambia de escena con Settings abierto
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Settings se carga encima de la escena actual y pausa el juego, así no se pierde la partida.
    /// </summary>
    public void OpenSettings()
    {
        if (SceneManager.GetSceneByName(SETTINGS_SCENE).isLoaded)
            return;

        Time.timeScale = 0f;
        SceneManager.LoadScene(SETTINGS_SCENE, LoadSceneMode.Additive);
    }

    public void CloseSettings()
    {
        Scene settings = SceneManager.GetSceneByName(SETTINGS_SCENE);
        if (!settings.isLoaded)
            return;

        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(settings);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene(LOSE_SCENE);
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene(WIN_SCENE);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
