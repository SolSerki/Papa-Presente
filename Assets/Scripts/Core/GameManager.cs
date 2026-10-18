using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }

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
        Debug.Log("//flush data from playerDeck");
        ChangeScene(MAIN_MENU_SCENE);

    }

    public void FinishRun()
    {
        Debug.Log("//switch to RunEnd scene");
        ChangeScene(RUN_END_SCENE);
        Debug.Log(" //win a card");
        Debug.Log("//show continue and end game buttons");
        
       
        
    }

    public void ContinueGameLoop()
    {
        
        
        Debug.Log("//add card to playerHand and save data");

        Debug.Log("//call Scene1");
        ChangeScene(SCENE_1);
    }

    public void StartNewGame()
    {
        Debug.Log("//load any data and flush any data");

        Debug.Log("//call Scene1");
        ChangeScene(SCENE_1);
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
