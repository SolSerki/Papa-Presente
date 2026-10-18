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
            Debug.LogWarning("[GameManager] No hay cartas de recompensa cargadas para este final.");

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
    }

    private CardData PickRandom(List<CardData> pool)
    {
        if (pool == null || pool.Count == 0)
            return null;

        return pool[Random.Range(0, pool.Count)];
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
