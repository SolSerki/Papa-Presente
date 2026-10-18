using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActionsManager : MonoBehaviour
{
    public void ContinueGameLoop()
    {
        GameManager.Instance.ContinueGameLoop();
    }

    public void EndGame()
    {
        GameManager.Instance.EndGame();

    }

    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void ChangeScene(string name)
    {
        GameManager.Instance.ChangeScene(name);
    }

    public void Exit()
    {
        GameManager.Instance.Exit();
    }

    public void OpenSettings()
    {
        GameManager.Instance.OpenSettings();
    }

    public void CloseSettings()
    {
        GameManager.Instance.CloseSettings();
    }
}
