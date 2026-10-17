using UnityEngine;

public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }
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
        //flush data from playerDeck

    }

    public void FinishRun()
    {
        //switch to RunEnd scene
        //win a card
        //show continue and end game buttons
    }
    
}
